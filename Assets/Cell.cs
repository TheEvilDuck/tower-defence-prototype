using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Transform placable;
    public bool hasRoad
    {
        get;
        private set;
    }

    public void BuildRoad()
    {
        hasRoad = true;
    }
}
