using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    [SerializeField]PlayerInput _playerInput;
    [SerializeField]TileBase _groundTileRule;
    [SerializeField]TileBase _roadTileRule;
    [SerializeField]Tilemap _tileMap;
    [SerializeField]Tilemap _roadMap;
    [SerializeField]GameObject _backGroundPrefab;
    [SerializeField]int _gridSize = 10;

    private bool _placeRoad = false;

    private Grid _grid;

    private void OnEnable() {
        _playerInput.mouseClickEvent+=OnMouseClicked;
        _playerInput.mouseRightClickEvent+=OnMouseRightClicked;
        _playerInput.switched+=Switch;
    }
    private void OnDisable() {
        _playerInput.mouseClickEvent-=OnMouseClicked;
        _playerInput.mouseRightClickEvent-=OnMouseRightClicked;
        _playerInput.switched-=Switch;
    }
    private void Start() {
        _grid = new Grid(_gridSize,1f);
        Transform backGround = Instantiate(_backGroundPrefab).transform;
        backGround.position = new Vector3(_gridSize/2f,_gridSize/2f);
        backGround.localScale = new Vector3(_gridSize,_gridSize);
    }
    private void OnMouseClicked(Vector2 position)
    {
        Vector2Int cellPos = _grid.WorldPositionToGridPosition(position);
        Cell cell = _grid.GetCellByPosition(cellPos);
        if (cell!=null)
        {
           if (_placeRoad)
           {
                if (cell.CellType!=TileEnum.Dirt)
                    return;
                _roadMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),_roadTileRule);
                _grid.GetCellByPosition(cellPos).BuildRoad();
           }
           else
           {
                _tileMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),_groundTileRule);
                _grid.ChangeCellType(cellPos,TileEnum.Dirt);
           }
        }
    }
    private void OnMouseRightClicked(Vector2 position)
    {
        Vector2Int cellPos = _grid.WorldPositionToGridPosition(position);
        Cell cell = _grid.GetCellByPosition(cellPos);
        if (cell!=null)
        {
            if (_placeRoad)
           {
                _roadMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),null);
                _grid.GetCellByPosition(cellPos).RemoveRoad();
           }
           else
           {
                if (cell.hasRoad)
                {
                    _roadMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),null);
                    _grid.GetCellByPosition(cellPos).RemoveRoad();
                }
                _tileMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),null);
                _grid.ChangeCellType(cellPos,TileEnum.Nothing);
           }
        }
    }
    private void Switch(int index)
    {
        switch (index)
        {
           case 0:
            _placeRoad = false;
           break;
           case 1:
            _placeRoad = true;
           break;
        }
    }
}
