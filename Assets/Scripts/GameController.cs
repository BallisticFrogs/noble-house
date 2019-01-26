using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public GameObject Grid;

    // Start is called before the first frame update
    void Awake()
    {
        Tilemap tilemap = Grid.GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // Recover Wall Tiles
        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];
                
                if (tile != null) {
                    if (tile.GetType() == typeof(WorldTile)) {
                        WorldTile wt = tile as WorldTile;
                        Debug.Log("x:" + x + " y:" + y + " tile:" + wt.Type);
                    }                    
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
