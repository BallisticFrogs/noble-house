using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using SettlersEngine;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;

    public GameObject Grid;
    public Tilemap tilemap;
    public PathNode[,] PathfindingMap;

    public SpatialAStar<PathNode, Character> aStar;

    // Start is called before the first frame update
    void Awake()
    {
        INSTANCE = this;
        tilemap = Grid.GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        // Debug.Log("bounds.x = " + bounds.x);
        // Debug.Log("bounds.y = " + bounds.y);
        // Debug.Log("bounds.xmax = " + bounds.xMax);
        // Debug.Log("bounds.xmin = " + bounds.xMin);

        // Debug.Log("0,0 = " + tilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0)));
        // Debug.Log("-1,-1 = " + tilemap.GetCellCenterWorld(new Vector3Int(-1, -1, 0)));
        // Debug.Log("origin = " + tilemap.origin);

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

        aStar = new SpatialAStar<PathNode, Character>(PathfindingMap);
    }

    public LinkedList<PathNode> getPath(int x, int y, int targetX, int targetY)
    {
        LinkedList<PathNode> path = aStar.Search(new Point(x, y), new Point(targetX, targetY), null);
        return path;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
