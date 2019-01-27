﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using SettlersEngine;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;

    public UnityEngine.Color SELECTEC_COLOR = new UnityEngine.Color(0, 1, 0);

    [HideInInspector]
    public Tilemap tilemap;

    public PathNode[,] PathfindingMap;
    public Pathfinder<PathNode, Character> aStar;

    [HideInInspector]
    public Servant selectedServant;

    private bool gameLost;

    private Dictionary<Noble, HoldableObject> activeTasks = new Dictionary<Noble, HoldableObject>();
    void Awake()
    {
        INSTANCE = this;
        InitPathfinding();
    }

    private void InitPathfinding()
    {
        tilemap = GameObject.FindGameObjectWithTag(Tags.TILEMAP).GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("No game object found with tag 'TilemapObstacles'");
        }

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // Declare Pathfinding Array
        PathfindingMap = new PathNode[bounds.size.x, bounds.size.y];

        // Recover Wall Tiles
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null && tile.GetType() == typeof(WorldTile))
                {
                    WorldTile wt = tile as WorldTile;

                    // Player can't walk in
                    PathfindingMap[x, y] = new PathNode(x, y, true);
                }
                else
                {
                    // Player can walk
                    PathfindingMap[x, y] = new PathNode(x, y, false);
                }
            }
        }

        aStar = new Pathfinder<PathNode, Character>(PathfindingMap);
    }

    public LinkedList<PathNode> getPath(int x, int y, int targetX, int targetY)
    {
        LinkedList<PathNode> path = aStar.Search(new Point(x, y), new Point(targetX, targetY), null);
        return path;
    }


    void Update()
    {
        // check lose condition
        GameObject[] servants = GameObject.FindGameObjectsWithTag(Tags.SERVANT);
        if ((servants == null || servants.Length == 0) && !gameLost)
        {
            gameLost = true;
            GameOverManager.INSTANCE.GameOverDefeat();
        }

        // handle servant selection/deselection
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedServant != null)
            {
                selectedServant.GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                selectedServant = null;
            }

            // raycast to find collider under mouse
            RaycastHit2D rayhit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (rayhit.collider != null)
            {
                var servant = rayhit.collider.gameObject.GetComponent<Servant>();
                if (servant != null)
                {
                    selectedServant = servant;
                    servant.GetComponent<SpriteRenderer>().color = SELECTEC_COLOR;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (selectedServant != null)
            {
                bool handled = DetectClickOnNoble();
                if (!handled)
                {
                    handled = DetectClickOnSpecialTile();
                }
                if (!handled)
                {
                    Vector3Int cellMouse = GameController.INSTANCE.tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    this.selectedServant.SetTarget(cellMouse);
                }
            }
        }
    }

    private bool DetectClickOnNoble()
    {
        // raycast to find collider under mouse
        RaycastHit2D rayhit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (rayhit.collider != null)
        {
            var noble = rayhit.collider.gameObject.GetComponent<Noble>();
            if (noble != null)
            {
                if ((noble.transform.position - selectedServant.transform.position).magnitude < 1.5f)
                {
                    selectedServant.InteractWithNoble(noble);
                }
                else
                {
                    // TODO move close to him
                }
                return true;
            }
        }
        return false;
    }

    private bool DetectClickOnSpecialTile()
    {
        // handle clics on special tiles
        Vector3Int cellMouse = GameController.INSTANCE.tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var tileClicked = GameController.INSTANCE.tilemap.GetTile(cellMouse);
        Vector3Int servantCoords = GameController.INSTANCE.tilemap.WorldToCell(selectedServant.transform.position);

        if (tileClicked != null && tileClicked.GetType() == typeof(WorldTile))
        {
            WorldTile wt = tileClicked as WorldTile;
            // Si mon perso est à côté du puits ou autre
            if (isServantCloseToTile(servantCoords, cellMouse))
            {
                selectedServant.HandleClicOnSpecialTile(wt);
            }
            return true;
        }
        return false;
    }

    private bool isServantCloseToTile(Vector3Int servantCoords, Vector3Int cellMouse)
    {
        if (Math.Abs(servantCoords.x - cellMouse.x) < 1.5 && Math.Abs(servantCoords.y - cellMouse.y) < 1.5)
        {
            return true;
        }
        return false;
    }

    public void AddActiveTasks(Noble noble, HoldableObject wish)
    {
        activeTasks.Add(noble, wish);
        UIManager.INSTANCE.AddTask(wish);
        // UIManager.INSTANCE.happynessLevel --;
        // UIManager.INSTANCE.angrynessLevel --;
        Debug.Log("New Task");
    }

    public void CompleteActiveTask(Noble noble)
    {
        HoldableObject wish;
        activeTasks.TryGetValue(noble, out wish);
        UIManager.INSTANCE.RemoveTask(wish);
        activeTasks.Remove(noble);
        UIManager.INSTANCE.happynessLevel--;
        UIManager.INSTANCE.angrynessLevel++;
        AngryCrowdManager.INSTANCE.addPeasant();
        Debug.Log("Task fullfilled!");
    }

    public void FailedActiveTask(Noble noble)
    {
        activeTasks.Remove(noble);
        UIManager.INSTANCE.happynessLevel++;
        UIManager.INSTANCE.angrynessLevel--;
        noble.OrderToKillServant();
        Debug.Log("Task failed!");
    }

}
