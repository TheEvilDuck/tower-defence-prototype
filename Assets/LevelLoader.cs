using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class LevelLoader
{
    public static readonly string mapsFolder = Application.dataPath+"/LevelEditor/Maps/";
    private float _cellSize = 1f;
    public LevelLoader(float cellSize)
    {
        _cellSize = cellSize;
    }
    public Grid LoadLevel(string fileName)
    {
        if (fileName==string.Empty)
            return null;
        string appPath = mapsFolder + fileName+".json";
        string Jsonstring = File.ReadAllText(appPath);
        GridData gridData = JsonUtility.FromJson<GridData>(Jsonstring);
        Grid grid = new Grid(gridData.xSize, _cellSize);

        for (int i = 0;i<gridData.grid.GetLength(0);i++)
        {
            int x = (i/gridData.xSize);
            int y = (i%gridData.xSize);

            grid.ChangeCellType(new Vector2Int(x,y),(TileEnum)gridData.grid[i]);
        }

        foreach(int roadIndex in gridData.roadIndexes)
        {
            int x = (roadIndex/gridData.xSize);
            int y = (roadIndex%gridData.xSize);

            grid.AddRoadToCell(new Vector2Int(x,y));
        }
        grid.UpdateRoadStartAndEnd();
        return grid;
    }
    
}
