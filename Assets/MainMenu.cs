using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]Button _startButton;
    [SerializeField]Button _levelEditorButton;
    [SerializeField]Button _exitGameButton;
    [SerializeField]GameObject _mainMenu;
    [SerializeField]GameObject _playMenu;

    [SerializeField]Button _playMenuStartButton;
    [SerializeField]Button _playMenuExitButton;
    [SerializeField]Transform _iconsParent;
    [SerializeField]Button _iconPrefab;
    private LevelLoader _levelLoader;
    private List<Button> _levelIcons;
    private string _fileName;

    private void Awake() {
        _levelLoader = new LevelLoader(1f);
        _levelIcons = new List<Button>();
        string[] levelNames = _levelLoader.GetAllLevelNames();
        foreach (string name in levelNames)
        {
            Button levelIcon = Instantiate(_iconPrefab,_iconsParent);
            levelIcon.onClick.AddListener(()=>{
                if (_fileName==name)
                    _fileName = string.Empty;
                else
                    _fileName = name;
            });
            Texture2D tex = _levelLoader.LoadLevelImage(name);
            levelIcon.image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            TextMeshProUGUI mapNameText = levelIcon.transform.GetComponentInChildren<TextMeshProUGUI>();
            mapNameText.text = name;
        }
    }
    private void OnEnable() 
    {
        _startButton.onClick.AddListener(EnablePlayMenu);
        _levelEditorButton.onClick.AddListener(LevelEditorEnter);
        _exitGameButton.onClick.AddListener(ExitTheGame);
        
        _playMenuStartButton.onClick.AddListener(StartTheGame);
        _playMenuExitButton.onClick.AddListener(ExitPlayMenu);

        

    }
    private void OnDisable() 
    {
        _startButton.onClick.RemoveListener(EnablePlayMenu);
        _levelEditorButton.onClick.RemoveListener(LevelEditorEnter);
        _exitGameButton.onClick.RemoveListener(ExitTheGame);

        _playMenuStartButton.onClick.RemoveListener(StartTheGame);
        _playMenuExitButton.onClick.RemoveListener(ExitPlayMenu);
    }
    private void LevelEditorEnter()
    {
        SceneManager.LoadScene("Level editor");
    }
    private void StartTheGame()
    {
        if (_levelLoader.LevelExists(_fileName))
        {
            PlayerPrefs.SetString("loadedLevelName",_fileName);
            SceneManager.LoadScene("Game");
        }
        
    }
    private void ExitTheGame()
    {
        Application.Quit();
    }
    private void EnablePlayMenu()
    {
        _mainMenu.SetActive(false);
        _playMenu.SetActive(true);
    }
    private void ExitPlayMenu()
    {
         _mainMenu.SetActive(true);
        _playMenu.SetActive(false);
    }
}
