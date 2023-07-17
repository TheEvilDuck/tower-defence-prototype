using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using TMPro;

public class LevelEditor : MonoBehaviour
{
    [SerializeField]PlayerInput _playerInput;
    [SerializeField]TileBase _groundTileRule;
    [SerializeField]TileBase _roadTileRule;
    [SerializeField]Tilemap _tileMap;
    [SerializeField]Tilemap _roadMap;
    [SerializeField]GameObject _backGroundPrefab;
    [SerializeField]TMP_InputField _inputField;
    [SerializeField]int _gridSize = 10;


    private bool _placeRoad = false;
    private string _fileName = string.Empty;
    private TileController _tileController;
    private LevelLoader _levelLoader;

    private Grid _grid;

    private void OnEnable() {
        _playerInput.mouseClickEvent+=OnMouseClicked;
        _playerInput.mouseRightClickEvent+=OnMouseRightClicked;
        _playerInput.switched+=Switch;
        _inputField.onEndEdit.AddListener(OnMapNameChanged);
    }
    private void OnDisable() {
        _playerInput.mouseClickEvent-=OnMouseClicked;
        _playerInput.mouseRightClickEvent-=OnMouseRightClicked;
        _playerInput.switched-=Switch;
        _inputField.onEndEdit.RemoveListener(OnMapNameChanged);
    }
    private void Start() {
        _grid = new Grid(_gridSize,1f);
        Transform backGround = Instantiate(_backGroundPrefab).transform;
        backGround.position = new Vector3(_gridSize/2f,_gridSize/2f);
        backGround.localScale = new Vector3(_gridSize,_gridSize);

        _tileController = new TileController(_tileMap,_roadMap,_groundTileRule,_roadTileRule);
        _grid.cellChanged+=_tileController.OnCellChanged;

        _levelLoader = new LevelLoader(1f);
    }
    private void OnMapNameChanged(string name)
    {
        _fileName = name;
    }
    private void OnMouseClicked(Vector2 position)
    {
        Vector2Int cellPos = _grid.WorldPositionToGridPosition(position);
        FillCell(cellPos);
    }
    private void DeleteOnCell(Vector2Int cellPos)
    {
        Cell cell = _grid.GetCellByPosition(cellPos);
        if (cell!=null)
        {
            if (_placeRoad)
            {
                _grid.RemoveRoadAtCell(cellPos);
            }
            else
            {
                if (cell.hasRoad)
                {
                    _grid.RemoveRoadAtCell(cellPos);
                }
                _grid.ChangeCellType(cellPos,TileEnum.Nothing);
            }
        }
    }
    private void FillCell(Vector2Int cellPos)
    {
        Cell cell = _grid.GetCellByPosition(cellPos);
        if (cell!=null)
        {
           if (_placeRoad)
           {
                if (cell.CellType!=TileEnum.Dirt)
                    return;
                _grid.AddRoadToCell(cellPos);
           }
           else
           {
                _grid.ChangeCellType(cellPos,TileEnum.Dirt);
           }
        }
    }
    private void OnMouseRightClicked(Vector2 position)
    {
        Vector2Int cellPos = _grid.WorldPositionToGridPosition(position);
        DeleteOnCell(cellPos);
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
           case 2:
           SaveLevel();
           break;
           case 3:
           LoadLevel();
           break;
           case 4:
           ClearAll();
           break;
        }
    }

    private void SaveLevel()
    {
        if (_fileName==string.Empty)
            return;
        List<int>roadIndexes = new List<int>();
        Cell[,]grid = _grid.GetGrid();
        int[] cells = new int[grid.GetLength(0)*grid.GetLength(0)];
        for (int x = 0;x<grid.GetLength(0);x++)
        {
            for (int y = 0;y<grid.GetLength(1);y++)
            {
                int index = (x*_gridSize)+y;
                cells[index] = (int)grid[x,y].CellType;
                if (grid[x,y].hasRoad)
                    roadIndexes.Add(index);
            }
        }
        GridData gridData = new GridData
        {
            xSize = grid.GetLength(0),
            ySize = grid.GetLength(1),
            grid = cells,
            roadIndexes = roadIndexes.ToArray()
        };
        string jsonResult = JsonUtility.ToJson(gridData);
        string appPath = Application.dataPath+"/LevelEditor/Maps/" + _fileName+".json";
        File.WriteAllText(appPath,jsonResult);
    }
    public void LoadLevel()
    {
        if (_fileName==string.Empty)
            return;
        _grid.cellChanged-=_tileController.OnCellChanged;
        _grid = _levelLoader.LoadLevel(_fileName);
        _tileController.RedrawAll(_grid);
        _grid.cellChanged+=_tileController.OnCellChanged;
    }
    private void ClearAll()
    {
        _placeRoad = false;
        for (int x = 0;x<_gridSize;x++)
        {
            for (int y = 0;y<_gridSize;y++)
            {
                DeleteOnCell(new Vector2Int(x,y));
            }
        }
    }
}
