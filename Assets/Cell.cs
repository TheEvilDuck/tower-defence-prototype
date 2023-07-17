using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Transform placable;
    public TileEnum CellType {get;private set;}
    public bool hasRoad
    {
        get;
        private set;
    }

    public Cell()
    {
        CellType = TileEnum.Nothing;
    }
    public Cell(TileEnum tileType)
    {
        CellType = tileType;
    }

    public void BuildRoad()
    {
        hasRoad = true;
    }
    public void RemoveRoad()
    {
        hasRoad = false;
    }
}
