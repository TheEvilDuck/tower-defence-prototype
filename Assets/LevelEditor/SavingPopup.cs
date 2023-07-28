using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SavingPopup : MonoBehaviour
{
    [SerializeField]Button _yesButton;
    [SerializeField]Button _noButton;
    [SerializeField]Button _cancelButton;

    private Action<bool> _callBack;

    private void OnEnable() 
    {
        _yesButton.onClick.AddListener(OnConfirm);
        _noButton.onClick.AddListener(OnDicline);
        _cancelButton.onClick.AddListener(OnCancel);
    }
    private void OnDisable() 
    {
        _yesButton.onClick.RemoveListener(OnConfirm);
        _noButton.onClick.RemoveListener(OnDicline);
        _cancelButton.onClick.RemoveListener(OnCancel);
    }

    public void ActivatePopup(Action<bool> callback)
    {
        _callBack = callback;
    }

    private void OnConfirm()
    {
        _callBack?.Invoke(true);
    }
    private void OnDicline()
    {
        _callBack?.Invoke(false);
    }
    private void OnCancel()
    {
        _callBack = null;
        gameObject.SetActive(false);
    }
}
