using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveEditor : MonoBehaviour
{
    [SerializeField]Button _nextWaveButton;
    [SerializeField]Button _addWaveButton;
    [SerializeField]Button _removeWaveButton;
    [SerializeField]Button _previousWave;
    [SerializeField]Button _addEnemiesButton;
    [SerializeField]GameObject _enemyOptionsParentPrefab;
    [SerializeField]TextMeshProUGUI _waveCountText;
    [SerializeField]EnemyOptions _enemyOptionsPrefab;
    [SerializeField] Transform _mainPanel;

    [SerializeField]int _maxWaves = 10;
    [SerializeField]int _maxEnemySlotsPerWave = 5;

    private int _currentWaveId = 0;
    private List<GameObject> _enemimesParents;
    private bool _changeToNewWhenAdd = true;

    private void OnEnable() 
    {
        _addWaveButton.onClick.AddListener(AddWave);
        _removeWaveButton.onClick.AddListener(RemoveWave);
        _nextWaveButton.onClick.AddListener(NextWave);
        _previousWave.onClick.AddListener(PreviousWave);
        _addEnemiesButton.onClick.AddListener(AddEnemies);
    }
    private void OnDisable() 
    {
        _addWaveButton.onClick.RemoveListener(AddWave);
        _removeWaveButton.onClick.RemoveListener(RemoveWave);
        _nextWaveButton.onClick.RemoveListener(NextWave);
        _previousWave.onClick.RemoveListener(PreviousWave);
        _addEnemiesButton.onClick.RemoveListener(AddEnemies);
    }
    private void Awake() 
    {
        _enemimesParents = new List<GameObject>();
        UpdateWavesCountText();
        _addEnemiesButton.gameObject.SetActive(false);
    }
    private void AddWave()
    {
        if (_enemimesParents.Count>=_maxWaves)
            return;
        int id = 0;
        if (_enemimesParents.Count>0)
            id = _currentWaveId+1;
        if (!_changeToNewWhenAdd)
            id = _currentWaveId;
        _enemimesParents.Insert(id,Instantiate(_enemyOptionsParentPrefab,_mainPanel));
        _addEnemiesButton.gameObject.SetActive(true);
        if (_enemimesParents.Count>1&&_changeToNewWhenAdd)
            NextWave();
        else
            UpdateWavesCountText();
    }
    private void RemoveWave()
    {
        if (_enemimesParents.Count==0)
            return;
        Destroy(_enemimesParents[_currentWaveId]);
        _enemimesParents.RemoveAt(_currentWaveId);
        if (_currentWaveId>=_enemimesParents.Count)
            _currentWaveId = _enemimesParents.Count-1;
        if (_currentWaveId<0)
            _currentWaveId = 0;
        if (_enemimesParents.Count>0)
            _enemimesParents[_currentWaveId].SetActive(true);
        else
            _addEnemiesButton.gameObject.SetActive(false);
        UpdateWavesCountText();
    }
    private void NextWave()
    {
        ChangeWave(1);
    }
    private void PreviousWave()
    {
        ChangeWave(-1);
    }
    private void ChangeWave(int offset)
    {
        if (_currentWaveId+offset<0)
            return;
        if (_currentWaveId+offset>=_enemimesParents.Count)
            return;
        _enemimesParents[_currentWaveId].SetActive(false);
        _currentWaveId+=offset;
        _enemimesParents[_currentWaveId].SetActive(true);
        UpdateWavesCountText();
    }
    private void UpdateWavesCountText()
    {
        int id = _currentWaveId+1;
        if (_enemimesParents.Count==0)
            id = 0;
        _waveCountText.text = $"{id}/{_enemimesParents.Count}";
    }
    private void AddEnemies()
    {
        Transform parent = _enemimesParents[_currentWaveId].transform;
        if (parent.childCount>=_maxEnemySlotsPerWave)
            return;
        EnemyOptions enemyOptions = Instantiate(_enemyOptionsPrefab,parent);
    }
    private void AddEnimiesWithEnemyData(EnemyData enemyData)
    {
        Transform parent = _enemimesParents[_currentWaveId].transform;
        if (parent.childCount>=_maxEnemySlotsPerWave)
            return;
        EnemyOptions enemyOptions = Instantiate(_enemyOptionsPrefab,parent);
        enemyOptions.LoadValues(enemyData);
    }
    public void LoadWaveData(WaveData[] waveDatas)
    {
        _changeToNewWhenAdd = false;
        while (_enemimesParents.Count>0)
            RemoveWave();
        _currentWaveId = 0;
        foreach (WaveData waveData in waveDatas)
        {
            AddWave();
            _enemimesParents[_currentWaveId].SetActive(false);
            foreach(EnemyData enemyData in waveData.enemies)
            {
                AddEnimiesWithEnemyData(enemyData);
            }
            _currentWaveId++;
        }
        _currentWaveId = 0;
        _enemimesParents[_currentWaveId].SetActive(true);
        UpdateWavesCountText();
        if (_enemimesParents.Count>0)
            _addEnemiesButton.gameObject.SetActive(true);
        _changeToNewWhenAdd = true;
    }
    public WaveData[] GetWaveData()
    {
        List<WaveData> result = new List<WaveData>();
        foreach (GameObject parent in _enemimesParents)
        {
            WaveData waveData = new WaveData();
            List<EnemyData> enemyDatas = new List<EnemyData>();
            if (parent.TryGetComponent<EnemiesParent>(out EnemiesParent enemiesParent))
            {
                waveData.timeToTheNextWave = enemiesParent.GetTimeToTheNextWave();
            }
            foreach (Transform child in parent.transform)
            {
                if (child.TryGetComponent<EnemyOptions>(out EnemyOptions enemyOptions))
                {
                    EnemyData enemyData = new EnemyData()
                    {
                        enemyId = enemyOptions.EnemyId,
                        statsMultiplier = enemyOptions.StatsMultiplier,
                        count = enemyOptions.EnemiesCount
                    };
                    enemyDatas.Add(enemyData);
                }
            }
            waveData.enemies = enemyDatas.ToArray();
            // TO DO : CHANGE IN LEVEL EDITOR
            waveData.timeToTheNextWave = 20f;
            result.Add(waveData);
        }
        return result.ToArray();
    }
}
