using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private Animator characterAnimator;

    private int _currentHealth;

    public int Money { get; private set; }


    private void Start()
    {
        _currentHealth = _health;
    }

    public void ApplyDamage(int damage)
    {
        if (gameObject.active)
        {

            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnEnemyDying(int reward)
    {
        Money += reward;
    }

    public void AddMoney(int money)
    {
        Money += money;
    }
}
