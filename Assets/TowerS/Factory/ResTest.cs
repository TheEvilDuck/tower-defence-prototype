using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResTest : PlacableOnGrid
{
    [SerializeField]float _rate=1f;
    [SerializeField]int _amount=1;

    private float _timer = 0;

    private PlayerStats _playerStats;

    public override void ProvidePlayerStats(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }

    public override void OnBuild()
    {
        
    }

    public override void OnRemove()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer>=_rate)
        {
            _timer = 0;
            _playerStats.AddMoney(_amount);
        }
        _timer+=Time.deltaTime;
    }
}
