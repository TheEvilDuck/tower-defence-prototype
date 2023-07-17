using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileController
{
    private Tilemap _tileMap;
    private Tilemap _roadMap;
    private TileBase _groundTileRule;
    private TileBase _roadTileRule;
    public TileController(Tilemap tileMap,Tilemap roadMap,TileBase groundTileRule, TileBase roadTileRule)
    {
        _tileMap = tileMap;
        _groundTileRule = groundTileRule;
        _roadTileRule = roadTileRule;
        _roadMap = roadMap;
    }

    public void FillCell(Vector2Int cellPos, bool placeRoad)
    {
        if (placeRoad)
            _roadMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),_roadTileRule);
        else
            _tileMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),_groundTileRule);
    }
    public void RemoveCell(Vector2Int cellPos, bool placeRoad)
    {
        if (placeRoad)
        {
            _roadMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),null);
            return;
        }
        _tileMap.SetTile(new Vector3Int(cellPos.x,cellPos.y),null);
    }

    public void OnCellChanged(Cell cell, Vector2Int cellPos)
    {
        if (cell.CellType==TileEnum.Nothing)
        {
            RemoveCell(cellPos, true);
            RemoveCell(cellPos, false);
            return;
        }
        if (_roadMap.GetTile(new Vector3Int(cellPos.x,cellPos.y))!=null&&!cell.hasRoad)
            RemoveCell(cellPos,true);
        if (_roadMap.GetTile(new Vector3Int(cellPos.x,cellPos.y))==null&&cell.hasRoad)
            FillCell(cellPos,true);
        if (_tileMap.GetTile(new Vector3Int(cellPos.x,cellPos.y))==null)
            FillCell(cellPos,false);
    }
    public void RedrawAll(Grid grid)
    {
        Cell[,] cells = grid.GetGrid();
        for (int x = 0;x<cells.GetLength(0);x++)
        {
            for (int y = 0;y<cells.GetLength(0);y++)
            {
                OnCellChanged(cells[x,y],new Vector2Int(x,y));
            }
        }
    }
}
