using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private Placable _placable;

    public Placable placable
    {
        get
        {
            return _placable;
        }
        private set{}
    }
    public bool TryAddPlacable(Placable placable)
    {
        if (IsOccupied())
            return false;
        _placable = placable;
        return true;
    }
    public bool IsOccupied()
    {
        return _placable!=null;
    }
}
