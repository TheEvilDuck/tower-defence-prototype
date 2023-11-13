using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : PlacableOnGrid
{
    [SerializeField]float _towerDamageMultiplier = 1.05f;

    private PlayerStats _playerStats;

    public override void ProvidePlayerStats(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }

    public override void OnBuild()
    {
        _playerStats.ModifyDamageMultiplier(_towerDamageMultiplier);
    }

    public override void OnRemove()
    {
        _playerStats.ModifyDamageMultiplier(-_towerDamageMultiplier);
    }
}
