using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField]List<PairEnemyCount> _enemies;
    [SerializeField]bool _randomized;
    [SerializeField]float _spawnRate;
    [SerializeField]float _timeToNextWave = 10f;

    private List<PairEnemyCount> _enemiesLeftToSpawn;

    public float SpawnRate{get => _spawnRate;}
    public float TimeToNextWave{get => _timeToNextWave;}

    public Wave(bool randomized,float spawnRate,float timeToNextWave,EnemyData[] enemimes,EnemiesPoolSO enemiesPool)
    {
        _randomized = randomized;
        _spawnRate = spawnRate;
        _timeToNextWave = timeToNextWave;
        _enemies = new List<PairEnemyCount>();
        foreach (EnemyData enemyData in enemimes)
        {
            PairEnemyCount pairEnemyCount = new PairEnemyCount();
            pairEnemyCount.count = enemyData.count;
            pairEnemyCount.enemy = enemiesPool.GetEnemyById(enemyData.enemyId);
            pairEnemyCount.statsMultiplier = enemyData.statsMultiplier;
            _enemies.Add(pairEnemyCount);
        }
    }
    public Enemy SpawnNext(DamagableComponent target)
    {
        if (_enemiesLeftToSpawn == null)
            _enemiesLeftToSpawn = new List<PairEnemyCount>(_enemies);
        if (_enemiesLeftToSpawn.Count==0)
            return null;
        int index = 0;
        if (_randomized)
            index = UnityEngine.Random.Range(0,_enemiesLeftToSpawn.Count-1);
        PairEnemyCount element = _enemiesLeftToSpawn[index];
        element.count-=1;
        if (_enemiesLeftToSpawn[index].count<=0)
            _enemiesLeftToSpawn.Remove(element);
        Enemy enemy = Object.Instantiate(element.enemy);
        enemy.Init(element.statsMultiplier,target);
        return enemy;
    }
    
}

[System.Serializable]
internal class PairEnemyCount
{
    [SerializeField] public Enemy enemy;
    [SerializeField] public int count;
    [SerializeField] public float statsMultiplier = 1f;

}
