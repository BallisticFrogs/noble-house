using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MonoBehaviour
{
    public Vector3Int target;

    public Vector3Int waypointCellCoords;

    void Start()
    {
        Vector3Int cellCoords = GameController.INSTANCE.tilemap.WorldToCell(transform.position);
        findNextWaypointToTarget(cellCoords);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            target = GameController.INSTANCE.tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        float threshold = 0.1f;
        Vector3 waypointWorldCoords = GameController.INSTANCE.tilemap.GetCellCenterWorld(waypointCellCoords);
        Vector3Int cellCoords = GameController.INSTANCE.tilemap.WorldToCell(transform.position);
        if ((waypointWorldCoords - transform.position).magnitude < threshold)
        {
            findNextWaypointToTarget(cellCoords);
        }
        else if (cellCoords != target)
        {
            findNextWaypointToTarget(cellCoords);
        }

        Vector3 dist = waypointWorldCoords - transform.position;
        if (dist.magnitude >= threshold * 0.1f)
        {
            Vector3 movement = dist.normalized * Math.Min(dist.magnitude, threshold);
            transform.position = transform.position + movement;
        }
    }

    private void findNextWaypointToTarget(Vector3Int currentCellCoords)
    {
        Debug.Log("Starting to pathfind...");
        var cellBounds = GameController.INSTANCE.tilemap.cellBounds;
        LinkedList<PathNode> path = GameController.INSTANCE.getPath(currentCellCoords.x - cellBounds.x, currentCellCoords.y - cellBounds.y,
            target.x - cellBounds.x, target.y - cellBounds.y);

        if (path != null && path.Count > 1)
        {
            PathNode waypointNode = path.First.Next.Value;
            waypointCellCoords = new Vector3Int(waypointNode.X + cellBounds.x, waypointNode.Y + cellBounds.y, 0);
            Debug.Log("found path : " + waypointNode.X + "-" + waypointNode.Y);
        }
        else
         if (path != null && path.Count > 0)
        {
            PathNode waypointNode = path.First.Value;
            waypointCellCoords = new Vector3Int(waypointNode.X + cellBounds.x, waypointNode.Y + cellBounds.y, 0);
            Debug.Log("found path : " + waypointNode.X + "-" + waypointNode.Y);
        }
    }

}
