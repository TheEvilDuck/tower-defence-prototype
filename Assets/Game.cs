using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
    [SerializeField]int _gridSize = 10;
    [SerializeField]float _cellCize = 1f;
    [SerializeField]Tilemap _tileMap;
    [SerializeField]Tilemap _roadMap;
    [SerializeField]TileBase _groundTileRule;
    [SerializeField]TileBase _roadTileRule;
    [SerializeField] GameObject _uiHandler;
    [SerializeField]EnemySpawner _enemiesSpawner;
    [SerializeField]int _startMoney = 100;

    [SerializeField]Base _basePrefab;

    private Base _base;

    public EnemySpawner EnemySpawner{get{
        return _enemiesSpawner;
    }}
    public Base Base  {get => _base;}

    public static Game instance;

    private Grid _grid;
    
    public PlayerStats PlayerStats{get; private set;}

    public Grid grid
    {
        get 
        {
            return _grid;
        }
        private set{}
    }
    
    public event Action gameOver;
    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    void Start()
    {
        PlayerStats = new PlayerStats(_startMoney);
        GridGenerator gridGenerator = new GridGenerator(_tileMap,_roadMap,_groundTileRule,_roadTileRule);
        _grid = gridGenerator.GenerateGrid(_gridSize,_cellCize);
        _base = Instantiate(_basePrefab,_grid.GetBasePosition(),Quaternion.identity);
        _base.GetComponent<DamagableComponent>().healthChanged+=((int value)=>{
            Debug.Log($"Base took damage, now has {value} health");
        });
        _base.GetComponent<DamagableComponent>().died+=(()=>{
            gameOver?.Invoke();
        });
        _uiHandler.SetActive(true);
    }
    
    
}
