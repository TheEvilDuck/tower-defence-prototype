using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEditor : MonoBehaviour
{
    [SerializeField]PlayerInput _playerInput;
    [SerializeField]TileBase _groundTileRule;
    [SerializeField]TileBase _roadTileRule;
    [SerializeField]Tilemap _tileMap;
    [SerializeField]Tilemap _roadMap;
    [SerializeField]GameObject _backGroundPrefab;
    [SerializeField]GameObject _wavesMenu;
    [SerializeField]TMP_InputField _inputField;
    [SerializeField]Button _wavesButton;
    [SerializeField]Button _exitButton;
    [SerializeField]int _gridSize = 10;
    [SerializeField] WaveEditor _waveEditor;
    [SerializeField]RenderTexture _renderTexture;
    [SerializeField]Camera _screenShotCamera;


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
        _wavesButton.onClick.AddListener(OnWavesButtonPressed);
        _exitButton.onClick.AddListener(OnExitButtonPress);
    }
    private void OnDisable() {
        _playerInput.mouseClickEvent-=OnMouseClicked;
        _playerInput.mouseRightClickEvent-=OnMouseRightClicked;
        _playerInput.switched-=Switch;
        _inputField.onEndEdit.RemoveListener(OnMapNameChanged);
        _wavesButton.onClick.RemoveListener(OnWavesButtonPressed);
        _exitButton.onClick.RemoveListener(OnExitButtonPress);
    }
    private void Start() {
        _grid = new Grid(_gridSize,1f);
        Transform backGround = Instantiate(_backGroundPrefab).transform;
        backGround.position = new Vector3(_gridSize/2f,_gridSize/2f);
        backGround.localScale = new Vector3(_gridSize,_gridSize);
        _screenShotCamera.transform.position = new Vector3(backGround.position.x,backGround.position.y,Camera.main.transform.position.z);

        _tileController = new TileController(_tileMap,_roadMap,_groundTileRule,_roadTileRule);
        _grid.cellChanged+=_tileController.OnCellChanged;

        _levelLoader = new LevelLoader(1f);
    }
    private void OnWavesButtonPressed()
    {
        _wavesMenu.SetActive(!_wavesMenu.activeSelf);
    }
    private void OnMapNameChanged(string name)
    {
        _fileName = name;
    }
    private void OnMouseClicked(Vector2 position)
    {
        if (_wavesMenu.activeSelf)
            return;
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
        if (_wavesMenu.activeSelf)
            return;
        Vector2Int cellPos = _grid.WorldPositionToGridPosition(position);
        DeleteOnCell(cellPos);
    }
    private void Switch(int index)
    {
        if (_wavesMenu.activeSelf)
            return;
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
        GridData gridDataToSave = new GridData
        {
            xSize = grid.GetLength(0),
            ySize = grid.GetLength(1),
            grid = cells,
            roadIndexes = roadIndexes.ToArray()
        };
        LevelData levelData = new LevelData
        {
            gridData = gridDataToSave,
            waves = _waveEditor.GetWaveData(),
            // TO DO CHANGE IN LEVEL EDITOR
            timeToTheFirstWave = 10f,
            startMoney = 100,
        };
        string jsonResult = JsonUtility.ToJson(levelData);
        string appPath = Application.dataPath+"/LevelEditor/Maps/" + _fileName+".json";
        File.WriteAllText(appPath,jsonResult);
        _screenShotCamera.gameObject.SetActive(true);
        _screenShotCamera.Render();

        RenderTexture.active = _renderTexture;
        Texture2D resultTexture = new Texture2D(_renderTexture.width,_renderTexture.height,TextureFormat.RGB24,false);
        resultTexture.ReadPixels(new Rect(0,0,_renderTexture.width,_renderTexture.height),0,0);
        resultTexture.Apply();

        byte[] bytes = resultTexture.EncodeToPNG();
        appPath = Application.dataPath+"/LevelEditor/Maps/" + _fileName+".png";
        File.WriteAllBytes(appPath,bytes);
        _screenShotCamera.gameObject.SetActive(false);


    }
    public void LoadLevel()
    {
        if (_fileName==string.Empty)
            return;
        _grid.cellChanged-=_tileController.OnCellChanged;
        LevelData levelData = _levelLoader.LoadLevel(_fileName);
        _grid = _levelLoader.GridDataToGrid(levelData.gridData);
        _tileController.RedrawAll(_grid);
        _grid.cellChanged+=_tileController.OnCellChanged;
        _waveEditor.LoadWaveData(levelData.waves);
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
    private void OnExitButtonPress()
    {
        SceneManager.LoadScene("Main menu");
    }
}
