using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placable : MonoBehaviour
{
    [SerializeField]Vector2Int[] _offsets;
    private List<Vector2Int>_occupiedCells;
    public List<Vector2Int>occupiedCells
    {
        get
        {
            return _occupiedCells;
        }
        private set{}
    }
    public void SetOccupiedSells(List<Vector2Int>occupiedCells)
    {
        _occupiedCells = occupiedCells;
    }
    public Vector2Int[]GetOffets()
    {
        return _offsets;
    }
}
