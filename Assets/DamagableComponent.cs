using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamagableComponent : MonoBehaviour
{
    [SerializeField]int _maxHealth;
    private int _health;

    public event Action<int>healthChanged;
    public event Action died;

    public int MaxHealth {get => _maxHealth;}
    public int Health {get =>_health;}
    
    private void Start() {
        _health = _maxHealth;
    }
    public void ChangeHealth(int amount)
    {
        int _prevHealth = _health;
        _health+=amount;
        if (_health<=0)
        {
            _health = 0;
            died?.Invoke();
        }
        if (_health>_maxHealth)
            _health = _maxHealth;
        if (_prevHealth != _health)
            healthChanged?.Invoke(_health);
    }
    public void ModifyMaxHealth(int amount)
    {
        _maxHealth+=amount;
        if (_maxHealth<0)
            _maxHealth = 0;
        if (_health>_maxHealth)
            ChangeHealth(_health-_maxHealth);
    }
    public void RestoreHealth()
    {
        if (_health<_maxHealth)
            ChangeHealth(_maxHealth-_health);
    }
}
