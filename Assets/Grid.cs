using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid
{
    private Cell[,] _grid;
    private int _size;
    private float _cellSize;

    public Vector2Int RoadStart{get;private set;}
    public Vector2Int RoadEnd{get;private set;}

    public Action<Cell,Vector2Int>cellChanged;

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
    public void Build(Vector2Int gridPos, PlacableOnGrid placable)
    {
        _grid[gridPos.x,gridPos.y].placable = placable;
    }
    public void  AddConstruction(Vector2Int gridPos, UnderConstruction underConstruction)
    {
        _grid[gridPos.x,gridPos.y].placable = underConstruction;
    }
    public bool TryRemovePlacableAt(Vector2Int gridPos, out PlacableOnGrid placableOnGrid)
    {   
        placableOnGrid = _grid[gridPos.x,gridPos.y].placable;
        if (_grid[gridPos.x,gridPos.y].placable!=null)
        {
            _grid[gridPos.x,gridPos.y].placable = null;
            return true;
        }
        return false;
    }
    public Vector2 GetBasePosition()
    {
        return RoadEnd+new Vector2(_cellSize*1.5f,_cellSize/2f);
    }
    public void ChangeCellType(Vector2Int pos, TileEnum tileType)
    {
        if (GetCellByPosition(pos)==null)
            return;
        Cell newCell = new Cell(tileType);
        if (newCell == _grid[pos.x,pos.y])
            return;
        _grid[pos.x,pos.y] = newCell;
        cellChanged?.Invoke(_grid[pos.x,pos.y],pos);
    }

    public void AddRoadToCell(Vector2Int pos)
    {
        if (GetCellByPosition(pos)==null)
            return;
        if (_grid[pos.x,pos.y].hasRoad)
            return;
        _grid[pos.x,pos.y].BuildRoad();
        UpdateRoadStartAndEnd();
        cellChanged?.Invoke(_grid[pos.x,pos.y],pos);
    }
    public void RemoveRoadAtCell(Vector2Int pos)
    {
        if (GetCellByPosition(pos)==null)
            return;
        if (!_grid[pos.x,pos.y].hasRoad)
            return;
        _grid[pos.x,pos.y].RemoveRoad();
        UpdateRoadStartAndEnd();
        cellChanged?.Invoke(_grid[pos.x,pos.y],pos);
    }

    private Vector2Int defineRoadStart(Cell[,] cells)
    {
        Vector2Int roadStart = Vector2Int.zero;
        for (int x = 0;x<cells.GetLength(0);x++)
        {
            for (int y = 0;y<cells.GetLength(1);y++)
            {
                if (cells[x,y].hasRoad)
                {
                    if (roadStart==Vector2Int.zero)
                    {
                        roadStart = new Vector2Int(x,y);
                        return roadStart;
                    }
                }
            }
        }
        return roadStart;
    }
    private Vector2Int defineRoadEnd(Cell[,] cells)
    {
        Vector2Int[] directions = {Vector2Int.down,Vector2Int.left,Vector2Int.right,Vector2Int.up};
        List<Vector2Int>visitedCells = new List<Vector2Int>();

        Vector2Int currentPos = RoadStart;
        while (true)
        {
            bool found = false;
            foreach (Vector2Int direction in directions)
            {
                Cell cell = GetCellByPosition(currentPos+direction);
                if (cell!=null)
                {
                    if (cell.hasRoad)
                    {
                        if (!visitedCells.Contains(currentPos+direction))
                        {
                            visitedCells.Add(currentPos);
                            currentPos = currentPos+direction;
                            found = true;
                            break;
                        }
                    }
                }
                
            }
            if (!found)
                break;
        }
        return currentPos;
    }

    public void UpdateRoadStartAndEnd()
    {
        RoadStart = defineRoadStart(_grid);
        RoadEnd = defineRoadEnd(_grid);
    }
}
