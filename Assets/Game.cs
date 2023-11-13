using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class Game : MonoBehaviour
{
    [SerializeField]int _gridSize = 10;
    [SerializeField]float _cellCize = 1f;
    [SerializeField]Tilemap _tileMap;
    [SerializeField]Tilemap _roadMap;
    [SerializeField]TileBase _groundTileRule;
    [SerializeField]TileBase _roadTileRule;
    [SerializeField] UIHandler _uiHandler;
    [SerializeField]EnemySpawner _enemiesSpawner;
    [SerializeField]Builder _builder;
    [SerializeField]int _baseMoneyPerTick = 5;

    [SerializeField]Base _basePrefab;

    private Base _base;

    public EnemySpawner EnemySpawner{get{
        return _enemiesSpawner;
    }}
    public Base Base  {get => _base;}

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
    void Start()
    {

        TileController tileController = new TileController(_tileMap,_roadMap,_groundTileRule,_roadTileRule);
        LevelLoader levelLoader = new LevelLoader(_cellCize);
        string levelName = PlayerPrefs.GetString("loadedLevelName");
        if (levelName==string.Empty||levelName==null)
            levelName = "test";
        LevelData levelData = levelLoader.LoadLevel(levelName);
        _grid = levelLoader.GridDataToGrid(levelData.gridData);
        _grid.cellChanged+=tileController.OnCellChanged;
        tileController.RedrawAll(_grid);

        PlayerStats = new PlayerStats(levelData.startMoney);

        _base = Instantiate(_basePrefab,_grid.GetBasePosition(),Quaternion.identity);
        _base.GetComponent<DamagableComponent>().healthChanged+=((int value)=>{
            Debug.Log($"Base took damage, now has {value} health");
        });
        _base.GetComponent<DamagableComponent>().died+=(()=>{
            gameOver?.Invoke();
        });
        _base.baseTickEvent+= () =>{
            PlayerStats.AddMoney(_baseMoneyPerTick);
        };
        
        _builder.Init(this);
        _enemiesSpawner.Init(levelData.timeToTheFirstWave,levelData.waves,gameOver,_grid,PlayerStats,_base);
        _uiHandler.Init(this);
    }
    
    
}
