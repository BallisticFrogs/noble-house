using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SettlersEngine;

public class GameController : MonoBehaviour
{
    public GameObject Grid;
    public PathNode[,] PathfindingMap;

    public SpatialAStar<PathNode, Character> aStar;

    // Start is called before the first frame update
    void Awake()
    {
        Tilemap tilemap = Grid.GetComponent<Tilemap>();

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

        aStar = new SpatialAStar<PathNode, Character>(PathfindingMap);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
