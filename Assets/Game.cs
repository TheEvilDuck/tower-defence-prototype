using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    [SerializeField]int _gridSize = 5;
    [SerializeField]float _cellCize = 1f;
    [SerializeField]Tilemap _tileMap;
    [SerializeField]Tilemap _roadMap;
    [SerializeField]TileBase _groundTileRule;
    [SerializeField]TileBase _roadTileRule;
    [SerializeField]Transform _towerPrefab;

    private Grid _grid;

    public Grid grid
    {
        get 
        {
            return _grid;
        }
        private set{}
    }
    void Start()
    {
        _grid = new Grid(_gridSize,_cellCize);
        Cell[,]_allCels = _grid.GetGrid();
        for (int x = 0;x<_gridSize;x++)
        {
            for (int y = 0;y<_gridSize;y++)
            {
                _tileMap.SetTile(new Vector3Int(x,y,0),_groundTileRule);
            }
        }
        
        for (int x = 0;x<_gridSize;x++)
        {
            int y = _gridSize/2;
            _roadMap.SetTile(new Vector3Int(x,y),_roadTileRule);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerClickedAtPosition(Vector2 position)
    {
       
    }
}
