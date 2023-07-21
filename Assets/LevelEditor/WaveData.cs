using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveData
{
    public float timeToTheNextWave;
    public EnemyData[] enemies;
}
