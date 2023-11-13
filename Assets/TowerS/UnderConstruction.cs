using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnderConstruction : PlacableOnGrid
{
    public event Action<bool> ConstructionDone;
    private float _timer = 0;
    private bool completed = false;

    public void SetTier(float totaltime)
    {
        _timer = totaltime;
        
    }
    public override void OnBuild()
    {
        
    }

    public override void OnRemove()
    {
        if (!completed)
            ConstructionDone?.Invoke(false);
        completed = true;
    }

    private void Update() {
        if (_timer <= 0)
        {
            if (completed)
                return;
            completed = true;
            ConstructionDone?.Invoke(true);
        }
        else _timer-=Time.deltaTime;
    }

}
