using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResTest : MonoBehaviour
{
    [SerializeField]float _rate=1f;
    [SerializeField]int _amount=1;

    private float _timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (_timer>=_rate)
        {
            _timer = 0;
            Game.instance.PlayerStats.AddMoney(_amount);
        }
        _timer+=Time.deltaTime;
    }
}
