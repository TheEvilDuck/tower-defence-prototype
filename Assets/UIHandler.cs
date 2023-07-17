using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI _moneyText;
    [SerializeField]GameObject _gameOverScreen;
    [SerializeField]TextMeshProUGUI _healthText;

    private void OnEnable() {
        Game.instance.PlayerStats.moneyChanged+=OnMoneyChanged;
        _moneyText.text = Game.instance.PlayerStats.Money.ToString();
        _healthText.text = "Health: "+Game.instance.Base.GetComponent<DamagableComponent>().MaxHealth.ToString();
        Game.instance.gameOver+=OnGameOver;
        Game.instance.Base.GetComponent<DamagableComponent>().healthChanged+=OnHealthChanged;
    }
    private void OnDisable() {
        Game.instance.PlayerStats.moneyChanged-=OnMoneyChanged;
        Game.instance.gameOver-=OnGameOver;
        Game.instance.Base.GetComponent<DamagableComponent>().healthChanged-=OnHealthChanged;
    }

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
}
