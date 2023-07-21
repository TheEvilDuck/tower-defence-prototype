using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyOptions : MonoBehaviour
{
    [SerializeField]TMP_InputField _enemiesCount;
    [SerializeField]TMP_Dropdown _enemyChoice;
    [SerializeField]TextMeshProUGUI _multiplierSliderText;
    [SerializeField]Slider _multiplierSlider;
    [SerializeField]Button _deleteButton;
    [SerializeField] EnemiesPoolSO _enemiesPool;


    public int EnemyId
    {
        get
        {
            return _enemyChoice.value;
        }
    }
    public float StatsMultiplier
    {
        get
        {
            return _multiplierSlider.value;
        }
    }
    public int EnemiesCount
    {
        get
        {
            if (_enemiesCount.text==string.Empty)
                return 1;
            if (int.TryParse(_enemiesCount.text,out int count))
                return count;
            return 1;
        }
    }
    private void Awake() {
        _multiplierSlider.value = 1;
        OnSliderValueChanged(1);
        Init();
        
    }
    private void Init()
    {
        if (_enemyChoice.options.Count>0)
            return;
        int count = _enemiesPool.EnemiesCount;
        List<TMP_Dropdown.OptionData>_optionDatas = new List<TMP_Dropdown.OptionData>();
        for (int i = 0;i<count;i++)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(_enemiesPool.GetEnemyById(i).name);
            _optionDatas.Add(optionData);
        }
        _enemyChoice.AddOptions(_optionDatas);
    }
    private void OnEnable() 
    {
        _multiplierSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _deleteButton.onClick.AddListener(OnDeleteButtonPressed);
    }
    private void OnDisable() 
    {
        _multiplierSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        _deleteButton.onClick.RemoveListener(OnDeleteButtonPressed);
    }

    private void OnSliderValueChanged(float value)
    {
        _multiplierSliderText.text = string.Format("{0:0.00}",value);
    }
    private void OnDeleteButtonPressed()
    {
        Destroy(gameObject);
    }
    public void LoadValues(EnemyData enemyData)
    {
        Init();
        _enemiesCount.text = enemyData.count.ToString();
        _enemyChoice.value = enemyData.enemyId;
        _multiplierSlider.value = enemyData.statsMultiplier;
        OnSliderValueChanged(_multiplierSlider.value);
    }
}
