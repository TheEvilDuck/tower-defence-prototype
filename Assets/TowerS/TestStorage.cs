using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStorage : MonoBehaviour
{
    [SerializeField]int _maxMoneyAddition = 10;
    void Start()
    {
        Game.instance.PlayerStats.AddMaxMoney(_maxMoneyAddition);
    }
}
