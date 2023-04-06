using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Add/Placable object")]
public class Placable : ScriptableObject
{
    [SerializeField]GameObject _prefab;
    public List<Vector2Int>occupiedCells = new List<Vector2Int>();

    [SerializeField]bool _canBePlacedOnRoad;
    public bool canBePlacedOnRoad
    {
        get
        {
            return _canBePlacedOnRoad;
        }
        private set{
            _canBePlacedOnRoad = value;
        }
    }
    public GameObject Init(Vector3 position)
    {
        return Instantiate(_prefab,position,Quaternion.identity);
    }
}
