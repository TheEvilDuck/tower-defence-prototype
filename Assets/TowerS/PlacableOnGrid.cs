using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlacableOnGrid : MonoBehaviour
{
    public virtual void ProvideBase(Base baseBuilding){}
    public virtual void ProvideEnemySpawner(EnemySpawner enemySpawner){}
    public virtual void ProvidePlayerStats(PlayerStats playerStats){}

    public abstract void OnBuild();
    public abstract void OnRemove();
}
