using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using SettlersEngine;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;

    public Tilemap tilemap;

    public PathNode[,] PathfindingMap;
    public Pathfinder<PathNode, Character> aStar;

    [HideInInspector]
    public Servant selectedServant;

    void Awake()
    {
        INSTANCE = this;
        InitPathfinding();
    }

    private void InitPathfinding()
    {
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
        // handle servant selection/deselection
        if (Input.GetMouseButtonDown(0))
        {
            // raycast to find collider under mouse
            RaycastHit2D rayhit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (rayhit.collider != null)
            {
                var servant = rayhit.collider.gameObject.GetComponent<Servant>();
                this.selectedServant = servant;
            }
            else
            {
                this.selectedServant = null;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (this.selectedServant != null)
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
                        selectedServant.handleClicOnSpecialTile(wt);
                    }
                    else
                    {
                        // FIXME will not work since pathfinding cannot get you to an obstacle tile
                        this.selectedServant.SetTarget(cellMouse);
                    }
                }
                else
                {
                    this.selectedServant.SetTarget(cellMouse);
                }
            }
        }
    }

    private bool isServantCloseToTile(Vector3Int servantCoords, Vector3Int cellMouse)
    {
        if (Math.Abs(servantCoords.x - cellMouse.x) < 1.5 && Math.Abs(servantCoords.y - cellMouse.y) < 1.5)
        {
            return true;
        }
        return false;
    }

}
