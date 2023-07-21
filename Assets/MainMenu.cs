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
    [SerializeField]TMP_InputField _inputField;
    private LevelLoader _levelLoader;

    private void Awake() {
        _levelLoader = new LevelLoader(1f);
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
        string fileName = _inputField.text;
        if (_levelLoader.LevelExists(fileName))
        {
            PlayerPrefs.SetString("loadedLevelName",fileName);
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
