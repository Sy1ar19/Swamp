using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(MoveState))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _reward;
    [SerializeField] private float _timeToDestroy;
    [SerializeField] private Player _target;

    private bool _isDead = false;
    private Animator _animator;
    private MoveState _moveState;

    public event UnityAction<Enemy> Dying;

    public bool IsDead => _isDead;
    public int Reward => _reward;

    public Player Target => _target;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _moveState = GetComponent<MoveState>();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _moveState.enabled = false;
            Dying?.Invoke(this);
            Die();
        }
    }

    public void Init(Player target)
    {
        _target = target;
    }

    public void Die()
    {
        _isDead = true;

        _animator.StopPlayback();
        _animator.Play("Death");

        StartCoroutine(DestroyAfterDelay(_timeToDestroy));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
