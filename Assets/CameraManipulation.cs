using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManipulation : MonoBehaviour
{
    [SerializeField]float _cameraSpeed = 1f;
    [SerializeField]PlayerInput _playerInput;
    private Vector2 _moveVector;
    private Transform _transform;

    private void OnEnable() {
        _playerInput.moveEvent+=UpdateMoveVector;
    }
    private void OnDisable() {
        _playerInput.moveEvent-=UpdateMoveVector;
    }
    private void Start() 
    {
        _transform = transform;
    }
    void Update()
    {
        _transform.Translate(_moveVector*_cameraSpeed);
    }

    public void UpdateMoveVector(Vector2 moveVector)
    {
        _moveVector = moveVector;
    }
}
