using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats
{
    public int Kills{get; private set;}
    public int  Money{get; private set;}

    public int MaxMoney{get;private set;}
    
    private float _towerDamageMultiplier = 1f;

    public float TowerDamageMultiplier
    {
        get => _towerDamageMultiplier;
        private set
        {
            if (value<=0)
                return;
            _towerDamageMultiplier = value;
        }
    }

    public event Action<int> moneyChanged;
    public event Action<int> maxMoneyChanged;
    public PlayerStats(int moneyByDefault)
    {
        Money = moneyByDefault;
        MaxMoney = moneyByDefault;
    }

    public void AddKill()
    {
        Kills++;
    }
    public void AddMoney(int money)
    {
        Money+=money;
        if (Money<0)
            Money = 0;
        if (Money>MaxMoney)
            Money = MaxMoney;
        moneyChanged?.Invoke(Money);
    }
    public void AddMaxMoney(int money)
    {
        MaxMoney+=money;
        if (MaxMoney<0)
            MaxMoney = 0;
        if (Money>MaxMoney)
            AddMoney(-Money+MaxMoney);
        maxMoneyChanged?.Invoke(MaxMoney);
    }
    public void ModifyDamageMultiplier(float amount)
    {
        TowerDamageMultiplier+=amount;
    }
}
