using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]EnemiesPoolSO _enemiesPool;
    float _firstWaveDelay = 10f;
    List<Wave>_waves;
    private int _currentWave = -1;
    private Coroutine _currentCoroutine;
    private PlayerStats _playerStats;
    private Grid _grid;
    private Base _base;

    public List<Enemy> enemies {get; private set;}

    public void Init(float firstWaveDelay, WaveData[] waves,Action gameOverEvent, Grid grid,PlayerStats playerStats,Base baseBuilding)
    {
        _base = baseBuilding;
        enemies = new List<Enemy>();
        _waves = new List<Wave>();
        _firstWaveDelay = firstWaveDelay;
        _playerStats = playerStats;
        _grid = grid;
        foreach (WaveData waveData in waves)
        {
            //TO DO CHANGE HARDCODED VALUES
            Wave wave = new Wave(true,1f,waveData.timeToTheNextWave,waveData.enemies,_enemiesPool);
            _waves.Add(wave);
        }
        _currentCoroutine = StartCoroutine(WaitForTheFirstWave());
        gameOverEvent+=(()=>{
            if (_currentCoroutine!=null)
                StopCoroutine(_currentCoroutine);
        });
    }
    public void StartNextWave()
    {
        _currentWave++;
        if (_currentWave>=_waves.Count||_waves.Count==0)
            return;
        _currentCoroutine = StartCoroutine(StartWave());
    }

    private IEnumerator WaitForTheFirstWave()
    {
        yield return new WaitForSeconds(_firstWaveDelay);
        StartNextWave();
    }
    private IEnumerator StartWave()
    {
        while (true)
        {
            Enemy enemy = _waves[_currentWave].SpawnNext(_base.GetComponent<DamagableComponent>());
            if (enemy == null)
                break;
            enemy.SetGrid(_grid);
            enemies.Add(enemy);
            DamagableComponent damagableComponent = enemy.GetComponent<DamagableComponent>();
            damagableComponent.died+=(()=>{
                enemies.Remove(enemy);
                Destroy(enemy.gameObject);
                _playerStats.AddKill();
                _playerStats.AddMoney(enemy.MoneyForKilling);
            });
            damagableComponent.healthChanged+=((int health)=>{
                //Debug.Log(health);
            });
            yield return new WaitForSeconds(_waves[_currentWave].SpawnRate);
        }
        yield return new WaitForSeconds(_waves[_currentWave].TimeToNextWave);
        StartNextWave();
    }

    
}
