using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]float _firstWaveDelay = 10f;
    [SerializeField]List<Wave>_waves;
    private int _currentWave = -1;
    private Coroutine _currentCoroutine;

    public List<Enemy> enemies {get; private set;}

    private void Start() {
        enemies = new List<Enemy>();
        _currentCoroutine = StartCoroutine(WaitForTheFirstWave());
        Game.instance.gameOver+=(()=>{
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
            Enemy enemy = _waves[_currentWave].SpawnNext();
            if (enemy == null)
                break;
            enemy.SetGrid(Game.instance.grid);
            enemies.Add(enemy);
            DamagableComponent damagableComponent = enemy.GetComponent<DamagableComponent>();
            damagableComponent.died+=(()=>{
                enemies.Remove(enemy);
                Destroy(enemy.gameObject);
                Game.instance.PlayerStats.AddKill();
                Game.instance.PlayerStats.AddMoney(enemy.MoneyForKilling);
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
