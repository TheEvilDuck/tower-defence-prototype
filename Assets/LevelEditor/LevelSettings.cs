using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSettings : MonoBehaviour
{
    [SerializeField]private TMP_InputField _levelName;
    [SerializeField]private Slider _moneySlider;
    [SerializeField]private Slider _timeToTheFirstWaveSlider;
    [SerializeField]private TextMeshProUGUI _moneyText;
    [SerializeField]private TextMeshProUGUI _timeToTheFirstWaveText;

    private void OnEnable() 
    {
        _moneySlider.onValueChanged.AddListener(OnMoneySliderChanged);
        _timeToTheFirstWaveSlider.onValueChanged.AddListener(OnTimeToTheFirstWaveSliderChanged);
    }
    private void OnDisable() 
    {
        _moneySlider.onValueChanged.RemoveListener(OnMoneySliderChanged);
        _timeToTheFirstWaveSlider.onValueChanged.RemoveListener(OnTimeToTheFirstWaveSliderChanged);
    }
    private void Start() {
        OnMoneySliderChanged(_moneySlider.value);
        OnTimeToTheFirstWaveSliderChanged(_timeToTheFirstWaveSlider.value);
    }
    public string LevelName
    {
        get => _levelName.text;
    }
    public int StartMoney
    {
        get => (int)_moneySlider.value*10;
    }
    public float TimeToTheFirstWave
    {
        get => _timeToTheFirstWaveSlider.value;
    }
    private void OnMoneySliderChanged(float value)
    {
        _moneyText.text = value.ToString();
        if (value>0)
            _moneyText.text+="0";
    }
    private void OnTimeToTheFirstWaveSliderChanged(float value)
    {
        _timeToTheFirstWaveText.text = value.ToString();
    }
}
