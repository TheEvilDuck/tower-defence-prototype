using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemiesParent : MonoBehaviour
{
    [SerializeField]Slider _slider;
    [SerializeField]TextMeshProUGUI _text;

    private void OnEnable() {
        _slider.onValueChanged.AddListener(SliderChanged);
    }
    private void OnDisable() {
        _slider.onValueChanged.RemoveListener(SliderChanged);
    }
    private void SliderChanged(float value)
    {
        _text.text = string.Format("{0:0.00}",value);
    }
    public float GetTimeToTheNextWave()
    {
        return _slider.value;
    }
}
