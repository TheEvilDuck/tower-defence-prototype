using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    private Vector2 _moveVector;

    public event Action<Vector2>moveEvent;
    public event Action<Vector2> mouseClickEvent;
    public event Action <Vector2> mouseRightClickEvent;
    public event Action<int> switched;
    
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
            moveEvent?.Invoke(_moveVector);
        }
        if (Input.GetMouseButtonDown(0))
        {
            mouseClickEvent?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetMouseButtonDown(1))
        {
            mouseRightClickEvent?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switched?.Invoke(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switched?.Invoke(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            switched?.Invoke(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            switched?.Invoke(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            switched?.Invoke(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            switched?.Invoke(5);
        }
        
    }
}