using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]float _moveSpeed = 1f;
    private Vector2 _moveVector;
    private Transform _transform;

    public Vector2 position
    {
        get
        {
            return _transform.position;
        }
        private set{}
    }

    private void Start()
    {
        _transform = transform;
    }

    private void Update() 
    {
        if (_moveVector.magnitude>0)
        {
            _transform.Translate(_moveVector*_moveSpeed);
        }
    }
    public void SetMoveVector(Vector2 value)
    {
        _moveVector = value;
    }
}
