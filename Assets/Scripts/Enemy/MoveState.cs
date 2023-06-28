using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class MoveState : State
{
    [SerializeField] private float _speed;
    [SerializeField] private Player _player;

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _runAnimationHash;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _runAnimationHash = Animator.StringToHash("Run");
    }

    private void FixedUpdate()
    {
        if (Target != null)
        {
            _agent.SetDestination(Target.transform.position);
        }
    }

    private void OnEnable()
    {
        _animator.Play(_runAnimationHash);
    }

    private void OnDisable()
    {
        _animator.StopPlayback();
    }
}
