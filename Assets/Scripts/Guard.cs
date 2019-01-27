using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Fighter
{

    private Vector3Int postingCellCoords;

    public override void Start()
    {
        base.Start();

        // save guarded position
        postingCellCoords = target;
    }

    public override void Update()
    {
        base.Update();

        // handle rampage time : charge !!!
        if (AngryCrowdManager.INSTANCE.rampaging && currentTarget == null)
        {
            currentTarget = AngryCrowdManager.INSTANCE.FindRandomPeasant();
        }

        // if no target and not in position, go back there
        if (currentTarget == null && postingCellCoords != GameController.INSTANCE.tilemap.WorldToCell(transform.position))
        {
            SetTarget(postingCellCoords);
        }
    }

}
