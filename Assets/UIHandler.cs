using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI _moneyText;
    [SerializeField]GameObject _gameOverScreen;
    [SerializeField]TextMeshProUGUI _healthText;
    [SerializeField]Button _towerMenuButton;
    [SerializeField]Transform _towerMenuPanel;

    private bool _isTowerMenuOpen = false;
    private Vector3 _towerMenuOpenPos;

    private void OnMoneyChanged(int money)
    {
        _moneyText.text = money.ToString();
    }
    private void OnGameOver()
    {
        _gameOverScreen.SetActive(true);
    }
    private void OnHealthChanged(int health)
    {
        _healthText.text = "Health: "+health.ToString();
    }
    private void OnTowerButtonClicked()
    {
        if (_isTowerMenuOpen)
            HideTowerMenu();
        else
            ShowTowerMenu();
    }
    private void HideTowerMenu()
    {
        _towerMenuPanel.localPosition = _towerMenuOpenPos+new Vector3(0,-_towerMenuPanel.lossyScale.y,0);
        _isTowerMenuOpen = false;
    }
    private void ShowTowerMenu()
    {
        _towerMenuPanel.localPosition = _towerMenuOpenPos;
        _isTowerMenuOpen = true;
    }
    public void Init(Game game)
    {
        game.PlayerStats.moneyChanged+=OnMoneyChanged;
        _moneyText.text = game.PlayerStats.Money.ToString();
        _healthText.text = "Health: "+game.Base.GetComponent<DamagableComponent>().MaxHealth.ToString();
        game.gameOver+=OnGameOver;
        game.Base.GetComponent<DamagableComponent>().healthChanged+=OnHealthChanged;
        _towerMenuButton.onClick.AddListener(OnTowerButtonClicked);
        _towerMenuOpenPos = _towerMenuPanel.localPosition;
        HideTowerMenu();
    }
}
