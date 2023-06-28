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

    private bool _canShoot = true;
    private float _elapsedTime = 0f;
    private float _shootInterval = 0.5f;

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
    }

    public override void Shoot(InputAction.CallbackContext context)
    {
        _firePoint.LookAt(_targetLook);

        if (_canShoot && context.phase == InputActionPhase.Started && _elapsedTime >= _shootInterval)
        {
            _elapsedTime = 0f;

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

            _canShoot = false;
            Invoke("ResetShootFlag", _shootInterval);
        }
    }

    private void ResetShootFlag()
    {
        _canShoot = true;
    }
}