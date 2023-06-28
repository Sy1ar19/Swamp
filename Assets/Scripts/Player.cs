using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Transform _shootPoint;

    private int _currentHealth;
    private Weapon _currentWeapon;
    public int Money { get; private set; }

    private void Start()
    {
        _currentHealth = _health;
        _currentWeapon = _weapons[0];
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
