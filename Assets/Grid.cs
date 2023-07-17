using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private Cell[,] _grid;
    private int _size;
    private float _cellSize;

    public Vector2Int RoadStart{get;private set;}
    public Vector2Int RoadEnd{get;private set;}

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
    public Vector2 GridPositionToWorldPosition(Vector2Int gridPosition)
    {
        float x = gridPosition.x*_cellSize+_cellSize/2f;
        float y = gridPosition.y*_cellSize+_cellSize/2f;
        return new Vector2(x,y);
    }

    public bool CheckPlacement(Vector2Int gridPos, Placable placable)
    {
        if (_grid[gridPos.x,gridPos.y].CellType==TileEnum.Nothing)
            return false;
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
    public void AddConstruction(Vector2Int gridPos, Transform underConstructionTransform)
    {
        _grid[gridPos.x,gridPos.y].placable = underConstructionTransform;
    }

    public void SetRoadStartAndEnd(Vector2Int start,Vector2Int end)
    {
        RoadStart = start;
        RoadEnd = end;
    }
    public Vector2 GetBasePosition()
    {
        return RoadEnd+new Vector2(_cellSize*1.5f,_cellSize/2f);
    }
    public void ChangeCellType(Vector2Int pos, TileEnum tileType)
    {
        if (GetCellByPosition(pos)==null)
            return;
        _grid[pos.x,pos.y] = new Cell(tileType);
    }
}
