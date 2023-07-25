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

    public bool LevelExists(string levelName)
    {
        if (levelName==string.Empty)
            return false;
        string appPath = mapsFolder + levelName+".json";
        if (!File.Exists(appPath))
            return false;
        return true;
    }
    public LevelData LoadLevel(string fileName)
    {
        string appPath = mapsFolder + fileName+".json";
        if (!LevelExists(fileName))
            return new LevelData();
        string Jsonstring = File.ReadAllText(appPath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(Jsonstring);
        return levelData;
    }

    public string[] GetAllLevelNames()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(mapsFolder);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.json");
        List<string>result = new List<string>();
        foreach (FileInfo fileInfo in fileInfos)
        {
            string name = fileInfo.Name;
            int jsonIndex = name.IndexOf(".json");
            string resultName = name.Remove(jsonIndex,name.Length-jsonIndex);
            result.Add(resultName);
        }
        return result.ToArray();
        
    }
    public Texture2D LoadLevelImage(string fileName)
    {
        string appPath = mapsFolder + fileName+".png";
        if (!LevelExists(fileName))
            return null;
        if (!File.Exists(appPath))
            return null;
        byte[] data = File.ReadAllBytes(appPath);
        Texture2D texture = new Texture2D(2,2);
        texture.LoadImage(data);
        return texture;
        
    }
    public Grid GridDataToGrid(GridData gridData)
    {
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
