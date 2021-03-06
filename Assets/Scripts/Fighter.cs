﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Character
{

    public float reach = 1.5f;
    public float dps = 10;
    public float hitsPerSec = 2;

    private float hitDelay = 0;

    protected Character currentTarget;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (hitDelay > 0)
        {
            hitDelay -= Time.deltaTime;
        }

        if (currentTarget != null)
        {
            // update target tile from target entity position in the world
            var targetTile = GameController.INSTANCE.tilemap.WorldToCell(currentTarget.transform.position);
            SetTarget(targetTile);

            // if in range, hit it
            if ((currentTarget.transform.position - transform.position).magnitude <= reach)
            {
                if (hitDelay <= 0)
                {
                    currentTarget.life -= dps / hitsPerSec;
                    hitDelay = 1f / hitsPerSec;
                    currentTarget.OnPhysicalHit();
                }

                if (currentTarget.life <= 0)
                {
                    // stop chasing
                    currentTarget = null;
                }
            }
        }
    }

    protected override void Move()
    {
        Vector3 waypointWorldCoords = GameController.INSTANCE.tilemap.GetCellCenterWorld(waypointCellCoords);
        Vector3 dist = waypointWorldCoords - transform.position;
        bool doMove = false;
        float maxMove = speed;
        if (currentTarget != null)
        {
            doMove = (currentTarget.transform.position - transform.position).magnitude >= reach * 0.8f;
            maxMove = Math.Min(speed, reach * 0.8f);
        }
        else
        {
            doMove = dist.magnitude >= speed * 0.1f;
        }

        if (doMove)
        {
            Vector3 movement = dist.normalized * Math.Min(dist.magnitude, maxMove);
            transform.position += movement;
        }
    }

    public void PleaseKill(Character character)
    {
        currentTarget = character;
    }

}
