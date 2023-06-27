using UnityEngine;
using UnityEngine.InputSystem;

public class SAR : Weapon
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Camera _camera;
    [SerializeField] private int _damage;
    [SerializeField] private int _spread;
    [SerializeField] private int _shootForce;
    [SerializeField] private Transform _targetLook;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private AudioClip _shotSFX;
    [SerializeField] private AudioSource _audioSource;

    private bool canShoot = true;

    public override void Shoot(InputAction.CallbackContext context)
    {
        _firePoint.LookAt(_targetLook);
        if (context.phase == InputActionPhase.Started)
        {
            _audioSource.PlayOneShot(_shotSFX);
            _muzzleFlash.Play();

            RaycastHit hit;

            Bullet bullet = Instantiate(Bullet, _firePoint.position, _firePoint.rotation);

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out Enemy enemy))
                {
                    Debug.Log("Enemy!!!!!!!!!!!");
                    enemy.TakeDamage(_damage);
                    Destroy(bullet, 1f);
                }
            }
            canShoot = false;

            Invoke("ResetShootFlag", bullet.BulletLifeTIme);
        }
    }

    private void ResetShootFlag()
    {
        canShoot = true;
    }
}
