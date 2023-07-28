using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class LevelEditor : MonoBehaviour
{
    [SerializeField]PlayerInput _playerInput;
    [SerializeField]SavingPopup _savingPopup;
    [SerializeField]TileBase _groundTileRule;
    [SerializeField]TileBase _roadTileRule;
    [SerializeField]Tilemap _tileMap;
    [SerializeField]Tilemap _roadMap;
    [SerializeField]GameObject _backGroundPrefab;
    [SerializeField]TMP_InputField _inputField;
    [SerializeField] WaveEditor _waveEditor;
    [SerializeField] LevelSettings _levelSettings;
    [SerializeField]RenderTexture _renderTexture;
    [SerializeField]Camera _screenShotCamera;
    [SerializeField]Button _iconPrefab;

    [Header("Menus parents")]
    [SerializeField]Transform _iconsParent;
    [SerializeField]GameObject _loadMenu;
    [SerializeField]GameObject _wavesMenu;
    [SerializeField]GameObject _settingsMenu;



    [Header("Buttons")]
    [SerializeField]Button _wavesButton;
    [SerializeField]Button _exitButton;
    [SerializeField]Button _saveButton;
    [SerializeField]Button _loadButton;
    [SerializeField]Button _reloadButton;
    [SerializeField]Button _settingsButton;

    [Header("Values for settings")]
    [SerializeField] int _uiLayerId = 5;
    [SerializeField]int _gridSize = 10;





    private bool _placeRoad = false;
    private string _fileName = string.Empty;
    private TileController _tileController;
    private LevelLoader _levelLoader;
    private bool _hasUnsavedChanges = false;

    private Grid _grid;

    private void OnEnable() {
        _playerInput.mouseClickEvent+=OnMouseClicked;
        _playerInput.mouseRightClickEvent+=OnMouseRightClicked;
        _playerInput.switched+=Switch;
        _inputField.onValueChanged.AddListener(OnMapNameChanged);
        _wavesButton.onClick.AddListener(OnWavesButtonPressed);
        _exitButton.onClick.AddListener(OnExitButtonPress);
        _saveButton.onClick.AddListener(SaveLevel);
        _loadButton.onClick.AddListener(OpenOrCloseMapsFolder);
        _reloadButton.onClick.AddListener(ClearAll);
        _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
    }
    private void OnDisable() {
        _playerInput.mouseClickEvent-=OnMouseClicked;
        _playerInput.mouseRightClickEvent-=OnMouseRightClicked;
        _playerInput.switched-=Switch;
        _inputField.onValueChanged.RemoveListener(OnMapNameChanged);
        _wavesButton.onClick.RemoveListener(OnWavesButtonPressed);
        _exitButton.onClick.RemoveListener(OnExitButtonPress);
        _saveButton.onClick.RemoveListener(SaveLevel);
        _loadButton.onClick.RemoveListener(OpenOrCloseMapsFolder);
        _reloadButton.onClick.RemoveListener(ClearAll);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonPressed);
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

        CreateLoadMenu();
    }
    private void OnSettingsButtonPressed()
    {
        _settingsMenu.SetActive(!_settingsMenu.activeSelf);
    }
    private void OnWavesButtonPressed()
    {
        _wavesMenu.SetActive(!_wavesMenu.activeSelf);
    }
    private void OnMapNameChanged(string name)
    {
        _fileName = name;
        SearchMaps();
    }
    private void OnMouseClicked(Vector2 position)
    {
        if (_wavesMenu.activeSelf)
            return;
        if (IsPointOverUI(position))
            return;
        Vector2Int cellPos = _grid.WorldPositionToGridPosition(position);
        FillCell(cellPos);
        _hasUnsavedChanges = true;
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
        if (IsPointOverUI(position))
            return;
        Vector2Int cellPos = _grid.WorldPositionToGridPosition(position);
        DeleteOnCell(cellPos);
        _hasUnsavedChanges = true;
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
        if (_levelSettings.LevelName==string.Empty)
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
            timeToTheFirstWave = _levelSettings.TimeToTheFirstWave,
            startMoney = _levelSettings.StartMoney
        };
        string jsonResult = JsonUtility.ToJson(levelData);
        string appPath = Application.dataPath+"/LevelEditor/Maps/" + _levelSettings.LevelName+".json";
        File.WriteAllText(appPath,jsonResult);
        _screenShotCamera.gameObject.SetActive(true);
        _screenShotCamera.Render();

        RenderTexture.active = _renderTexture;
        Texture2D resultTexture = new Texture2D(_renderTexture.width,_renderTexture.height,TextureFormat.RGB24,false);
        resultTexture.ReadPixels(new Rect(0,0,_renderTexture.width,_renderTexture.height),0,0);
        resultTexture.Apply();

        byte[] bytes = resultTexture.EncodeToPNG();
        appPath = Application.dataPath+"/LevelEditor/Maps/" + _levelSettings.LevelName+".png";
        File.WriteAllBytes(appPath,bytes);
        _screenShotCamera.gameObject.SetActive(false);

        CreateLoadMenu();
    }
    private void SearchMaps()
    {
        foreach (Transform child in _iconsParent)
        {
            if (_fileName==string.Empty||child.name.ToLower().Contains(_fileName.ToLower()))
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }
    }
    private void OpenOrCloseMapsFolder()
    {
        _loadMenu.SetActive(!_loadMenu.activeSelf);
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
        _waveEditor.ClearCurrentWaveData();
    }
    private void OnExitButtonPress()
    {
        SceneManager.LoadScene("Main menu");
    }

    private void CreateLoadMenu()
    {
        foreach (GameObject child in _iconsParent)
        {
            Destroy(child);
        }
        string[] levelNames = _levelLoader.GetAllLevelNames();
        foreach (string name in levelNames)
        {
            Button levelIcon = Instantiate(_iconPrefab,_iconsParent);
            levelIcon.name = name;
            levelIcon.onClick.AddListener(()=>
            {
                _fileName = name;
                ActivateAskingSavePopup(LoadLevel,LoadLevel);
                _fileName = string.Empty;
            });
            Texture2D tex = _levelLoader.LoadLevelImage(name);
            levelIcon.image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            TextMeshProUGUI mapNameText = levelIcon.transform.GetComponentInChildren<TextMeshProUGUI>();
            mapNameText.text = name;
        }
    }
    private void ActivateAskingSavePopup(Action OnConfirm,Action OnDecline)
    {
        if (_hasUnsavedChanges)
                {
                    _savingPopup.gameObject.SetActive(true);
                    _savingPopup.ActivatePopup((bool result)=>{
                        if (result)
                        {
                            SaveLevel();
                        }
                        OnConfirm?.Invoke();
                        _savingPopup.gameObject.SetActive(false);
                    }
                    );
                }
                else
                {
                    OnDecline?.Invoke();
                }
    }
    private bool IsPointOverUI(Vector2 position)
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
