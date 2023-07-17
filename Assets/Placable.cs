using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Add/Placable object")]
public class Placable : ScriptableObject
{
    [SerializeField]GameObject _prefab;
    [SerializeField]int _cost;
    [SerializeField]float _buildTime;

    public int Cost{get{
        return _cost;
    }}
    public float BuildTime
    {
        get{
            return _buildTime;
        }
    }
    public List<Vector2Int>occupiedCells = new List<Vector2Int>();

    [SerializeField]bool _canBePlacedOnRoad;
    public bool canBePlacedOnRoad
    {
        get
        {
            return _canBePlacedOnRoad;
        }
    }

    public bool CompareCost(PlayerStats playerStats)
    {
        return playerStats.Money>=_cost;
    }
    public GameObject Init(Vector3 position)
    {
        return Instantiate(_prefab,position,Quaternion.identity);
    }
}
