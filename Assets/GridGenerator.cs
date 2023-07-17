using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGenerator
{
    private Tilemap _tileMap;
    private Tilemap _roadMap;
    private TileBase _groundTileRule;
    private TileBase _roadTileRule;
    public GridGenerator(Tilemap tileMap,Tilemap roadMap,TileBase groundTileRule, TileBase roadTileRule)
    {
        _tileMap = tileMap;
        _groundTileRule = groundTileRule;
        _roadTileRule = roadTileRule;
        _roadMap = roadMap;
    }
    public Grid GenerateGrid(int gridSize, float CellSize)
    {
        Grid grid =  new Grid(gridSize,CellSize);
        Cell[,]_allCels = grid.GetGrid();
        for (int x = 0;x<gridSize;x++)
        {
            for (int y = 0;y<gridSize;y++)
            {
                _tileMap.SetTile(new Vector3Int(x,y,0),_groundTileRule);
            }
        }
        int yRoad = gridSize/2;
        for (int x = 0;x<gridSize;x++)
        {
            
            grid.GetCellByPosition(new Vector2Int(x,yRoad)).BuildRoad();
            _roadMap.SetTile(new Vector3Int(x,yRoad),_roadTileRule);
        }
        //grid.SetRoadStartAndEnd(new Vector2Int(0,yRoad),new Vector2Int(gridSize-1,yRoad));
        return grid;
    }
}
