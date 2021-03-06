﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour
{
    public float speed = 0.2f;

    protected Vector3Int target;

    protected Vector3Int waypointCellCoords;

    public virtual void Start()
    {
        target = GameController.INSTANCE.tilemap.WorldToCell(transform.position);
        waypointCellCoords = target;
    }

    public virtual void Update()
    {
        Vector3 waypointWorldCoords = GameController.INSTANCE.tilemap.GetCellCenterWorld(waypointCellCoords);
        Vector3Int cellCoords = GameController.INSTANCE.tilemap.WorldToCell(transform.position);
        if (waypointCellCoords != target && (waypointWorldCoords - transform.position).magnitude < speed)
        {
            findNextWaypointToTarget(cellCoords);
        }

        Move();
    }

    protected virtual void Move()
    {
        Vector3 waypointWorldCoords = GameController.INSTANCE.tilemap.GetCellCenterWorld(waypointCellCoords);
        Vector3 dist = waypointWorldCoords - transform.position;
        if (dist.magnitude >= speed * 0.1f)
        {
            Vector3 movement = dist.normalized * Math.Min(dist.magnitude, speed);
            // movement.z = 0f;
            transform.position += movement;
        }
    }

    private void findNextWaypointToTarget(Vector3Int currentCellCoords)
    {
        // Debug.Log("Starting to pathfind...");
        var cellBounds = GameController.INSTANCE.tilemap.cellBounds;
        LinkedList<PathNode> path = GameController.INSTANCE.getPath(currentCellCoords.x - cellBounds.x, currentCellCoords.y - cellBounds.y,
            target.x - cellBounds.x, target.y - cellBounds.y);

        if (path != null && path.Count > 1)
        {
            PathNode waypointNode = path.First.Next.Value;
            waypointCellCoords = new Vector3Int(waypointNode.X + cellBounds.x, waypointNode.Y + cellBounds.y, 0);
            // Debug.Log("found path : " + waypointNode.X + "-" + waypointNode.Y);
        }
        else if (path != null && path.Count > 0)
        {
            PathNode waypointNode = path.First.Value;
            waypointCellCoords = new Vector3Int(waypointNode.X + cellBounds.x, waypointNode.Y + cellBounds.y, 0);
            // Debug.Log("found path : " + waypointNode.X + "-" + waypointNode.Y);
        }
    }

    public virtual void SetTarget(Vector3Int newTarget)
    {
        this.target = newTarget;
    }

}
