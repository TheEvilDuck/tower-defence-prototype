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
    public bool AddPlacableAtPosition(Placable placable,Vector2Int position)
    {
        if (_grid[position.x,position.y].IsOccupied())
            return false;
        Vector2Int[]offsets = placable.GetOffets();
        if (offsets!=null)
        {
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int offsetPosition = position+offset;
                if (offsetPosition.x<0||offsetPosition.y<0||offsetPosition.x>=_size||offsetPosition.y>=_size)
                    return false;
                if (_grid[offsetPosition.x,offsetPosition.y].IsOccupied())
                    return false;
            }
        }
        _grid[position.x,position.y].TryAddPlacable(placable);
        if (offsets!=null)
        {
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int offsetPosition = position+offset;
                _grid[offsetPosition.x,offsetPosition.y].TryAddPlacable(placable);
            }
        }
        return true;
    }
}
