using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class MoveState : State
{
    [SerializeField] private float _speed;
    [SerializeField] private Player _player;

    private NavMeshAgent _agent;
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        /*Vector3 currentTargetPosition = Target.transform.position;
        currentTargetPosition.y = this.transform.position.y;*/
        _agent.SetDestination(_player.transform.position);

/*        if (Target.transform != null)
        {
            _agent.SetDestination(Target.transform.position);
            print(Target.transform.position);
        }*/
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
