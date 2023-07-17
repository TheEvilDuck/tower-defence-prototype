using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamagableComponent))]
public class Base : MonoBehaviour
{
    private DamagableComponent _damageComponent;

    public DamagableComponent DamagableComponent {get=>_damageComponent;}

    private void Awake() {
        _damageComponent = GetComponent<DamagableComponent>();
    }
}
