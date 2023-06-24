using UnityEngine;

[RequireComponent (typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
