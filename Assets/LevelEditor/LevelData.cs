using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    [SerializeField] public WaveData[] waves;
    [SerializeField] public GridData gridData;
    [SerializeField] public int startMoney;
    [SerializeField] public float timeToTheFirstWave;
}
