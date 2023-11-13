using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStorage : PlacableOnGrid
{
    [SerializeField]int _maxMoneyAddition = 10;
    private PlayerStats _playerStats;

    public override void ProvidePlayerStats(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }

    public override void OnBuild()
    {
        _playerStats.AddMaxMoney(_maxMoneyAddition);
    }

    public override void OnRemove()
    {
        _playerStats.AddMaxMoney(-_maxMoneyAddition);
    }

}
