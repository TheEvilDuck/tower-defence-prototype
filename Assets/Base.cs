using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamagableComponent))]
public class Base : MonoBehaviour
{
    private DamagableComponent _damageComponent;

    private float _timer = 0;

    public DamagableComponent DamagableComponent {get=>_damageComponent;}

    public event Action baseTickEvent;

    private void Awake() {
        _damageComponent = GetComponent<DamagableComponent>();
    }

    private void Update() {
        if (_damageComponent.Health==0)
            return;
        if (_timer>=1f)
        {
            _timer = 0;
            baseTickEvent?.Invoke();
        }
        _timer+=Time.deltaTime;
    }
}
