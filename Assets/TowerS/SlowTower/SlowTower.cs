using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [SerializeField]float _slowMuiltiplier = 0.8f;

    public override void Hit()
    {
        base.Hit();
        _target.MultiplySpeed(_slowMuiltiplier);
    }
}
