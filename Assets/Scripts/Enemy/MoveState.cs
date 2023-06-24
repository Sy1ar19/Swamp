using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class MoveState : State
{
    [SerializeField] private float _speed;
    [SerializeField] private Player _target;

    private NavMeshAgent _agent;
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(Target != null)
        {
            _agent.SetDestination(_target.transform.position);
        }
    }

    private void OnEnable()
    {
        _animator.Play("Run");
    }

    private void OnDisable()
    {
        _animator.StopPlayback();
    }
}
