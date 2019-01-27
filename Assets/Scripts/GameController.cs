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
    public GameObject uiManager;

    public UnityEngine.Color SELECTEC_COLOR = new UnityEngine.Color(0, 1, 0);

    [HideInInspector]
    public Tilemap tilemap;

    public PathNode[,] PathfindingMap;
    public Pathfinder<PathNode, Character> aStar;

    [HideInInspector]
    public Servant selectedServant;

    public float guardCorruptionDelay = 5;
    [HideInInspector]
    public bool internalPurge;

    private bool gameOver;

    void Awake()
    {
        INSTANCE = this;
        InitPathfinding();
        uiManager.SetActive(true);
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
        if (gameOver) return;

        //  check lose condition
        GameObject[] servants = GameObject.FindGameObjectsWithTag(Tags.SERVANT);
        if ((servants == null || servants.Length == 0) && !gameOver)
        {
            gameOver = true;
            GameOverManager.INSTANCE.GameOverDefeat();
        }

        // check win condition
        GameObject[] nobles = GameObject.FindGameObjectsWithTag(Tags.NOBLE);
        if (!AngryCrowdManager.INSTANCE.rampaging && (nobles == null || nobles.Length == 0) && !gameOver)
        {
            gameOver = true;
            GameOverManager.INSTANCE.GameOverVictory();
        }

        UIManager.INSTANCE.updateListItem(GameObject.FindGameObjectsWithTag(Tags.NOBLE));

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
                bool handled = DetectClickOnNobleOrGuard();
                if (!handled)
                {
                    handled = DetectClickOnSpecialTile();
                }
                if (!handled)
                {
                    Vector3Int cellMouse = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    this.selectedServant.SetTarget(cellMouse);
                }
            }
        }
    }

    private bool DetectClickOnNobleOrGuard()
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
                    // clic directly on it : move close to him
                    Vector3Int cell = tilemap.WorldToCell(noble.transform.position);
                    cell = FindClosestFreeCellNear(cell, noble);
                    selectedServant.SetTarget(cell);
                }
                return true;
            }

            var guard = rayhit.collider.gameObject.GetComponent<Guard>();
            if (guard != null)
            {
                if ((guard.transform.position - selectedServant.transform.position).magnitude < 1.5f)
                {
                    selectedServant.InteractWithguard(guard);
                }
                else
                {
                    // clic directly on it : move close to him
                    Vector3Int cell = tilemap.WorldToCell(guard.transform.position);
                    cell = FindClosestFreeCellNear(cell, guard);
                    selectedServant.SetTarget(cell);
                }
                return true;
            }
        }
        return false;
    }

    private bool DetectClickOnSpecialTile()
    {
        // handle clics on special tiles
        Vector3Int cellMouse = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var tileClicked = tilemap.GetTile(cellMouse);
        Vector3Int servantCoords = tilemap.WorldToCell(selectedServant.transform.position);

        if (tileClicked != null && tileClicked.GetType() == typeof(WorldTile))
        {
            WorldTile wt = tileClicked as WorldTile;
            // Si mon perso est à côté du puits ou autre
            if (isServantCloseToTile(servantCoords, cellMouse))
            {
                selectedServant.HandleClicOnSpecialTile(wt);
            }
            else
            {
                // clic directly on it : move close to him
                var cell = FindClosestFreeCellNear(cellMouse, selectedServant);
                selectedServant.SetTarget(cell);
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

    public void CompleteActiveTask(Noble noble)
    {
        AngryCrowdManager.INSTANCE.addPeasants();
        Debug.Log("Task fullfilled!");
    }

    public void FailedActiveTask(Noble noble)
    {
        noble.OrderToKillClosestServant();
        Debug.Log("Task failed!");
    }

    public Vector3Int GetAgonizingMoveTarget(Character character)
    {
        Vector3Int cell = tilemap.WorldToCell(character.transform.position);
        return FindRandomFreeCellNear(cell);
    }

    private Vector3Int FindClosestFreeCellNear(Vector3Int cell, Character character)
    {
        Vector3Int closestCell = cell;
        float dist = float.MaxValue;

        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                var testCell = new Vector3Int(cell.x + i, cell.y + j, 0);
                TileBase tile = tilemap.GetTile(testCell);
                bool hasObstacle = tile != null && tile.GetType() == typeof(WorldTile);
                if (!hasObstacle)
                {
                    Vector3 testCellPos = tilemap.GetCellCenterWorld(testCell);
                    float d = (character.transform.position - testCellPos).magnitude;
                    if (d <= dist)
                    {
                        dist = d;
                        closestCell = testCell;
                    }
                }
            }
        }

        return closestCell;
    }

    private Vector3Int FindRandomFreeCellNear(Vector3Int cell)
    {
        Vector3Int closestCell = cell;
        float dist = float.MaxValue;

        for (int i = -2; i <= 2; i += 1)
        {
            for (int j = -2; j <= 2; j += 1)
            {
                if (i == 0 && j == 0) continue;

                var testCell = new Vector3Int(cell.x + i, cell.y + j, 0);
                TileBase tile = tilemap.GetTile(testCell);
                bool hasObstacle = tile != null && tile.GetType() == typeof(WorldTile);
                if (!hasObstacle)
                {
                    Vector3 testCellPos = tilemap.GetCellCenterWorld(testCell);
                    float d = UnityEngine.Random.Range(0, 100);
                    if (d <= dist)
                    {
                        dist = d;
                        closestCell = testCell;
                    }
                }
            }
        }

        return closestCell;
    }

}
