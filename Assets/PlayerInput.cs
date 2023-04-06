using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]private MoveEvent _moveEvent;
    [SerializeField]private MoveEvent _mouseClicked;
    private Vector2 _moveVector;
    
    private void Start() 
    {
        _moveVector = new Vector2(0,0);
    }

    void Update()
    {
        Vector2 newMoveVector = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        if (_moveVector!=newMoveVector)
        {
            _moveVector = newMoveVector;
            _moveEvent?.Invoke(_moveVector);
        }
        if (Input.GetMouseButtonDown(0))
        {
            _mouseClicked?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        
    }
}
[System.Serializable]
public class MoveEvent:UnityEvent<Vector2>{}