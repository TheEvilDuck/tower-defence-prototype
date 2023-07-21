using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemiesPool")]
public class EnemiesPoolSO : ScriptableObject
{
    [SerializeField]List<Enemy> _enemies;

    public int EnemiesCount
    {
        get
        {
            return _enemies.Count;
        }
    }
    public Enemy GetEnemyById(int id)
    {
        if (id<0)
            return null;
        if (id>=_enemies.Count)
            return null;
        return _enemies[id];
    }
}
