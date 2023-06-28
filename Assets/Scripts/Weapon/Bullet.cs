using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _speed;
    [SerializeField] private float _bulletLifetime = 3f;

    public int Damage => _damage;
    public float BulletLifeTime => _bulletLifetime;

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        Destroy(this.gameObject, _bulletLifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            Destroy(this.gameObject);
        }
    }
}
