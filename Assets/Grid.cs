using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private Cell[,] _grid;
    private int _size;
    private float _cellSize;

    public Grid(int size,float cellSize)
    {
        _grid = new Cell[size,size];
        _size = size;
        _cellSize = cellSize;

        for (int x = 0;x< size;x++)
        {
            for (int y = 0;y<size;y++)
            {
                _grid[x,y] = new Cell();
            }
        }
    }

    public Cell GetCellByPosition(Vector2Int position)
    {
        if (position.x>=0&&position.x<_size&&position.y>=0&&position.y<_size)
            return _grid[position.x,position.y];
        return null;
    }
    public Cell[,] GetGrid()
    {
        return _grid;
    }

    public Vector2Int WorldPositionToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x/_cellSize);
        int y = Mathf.FloorToInt(worldPosition.y/_cellSize);
        return new Vector2Int(x,y);
    }

    public bool CheckPlacement(Vector2Int gridPos, Placable placable)
    {
        if (_grid[gridPos.x,gridPos.y].placable!=null)
            return false;
        if (_grid[gridPos.x,gridPos.y].hasRoad&&!placable.canBePlacedOnRoad)
            return false;
        return true;
    }
    public void Build(Vector2Int gridPos, Placable placable)
    {
        Vector3 position = new Vector3(gridPos.x+_cellSize/2f,gridPos.y+_cellSize/2f,0);
        _grid[gridPos.x,gridPos.y].placable = placable.Init(position).transform;
    }
}
