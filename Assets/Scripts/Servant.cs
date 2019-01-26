using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : Character
{
    private Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Coord dans la grille
            Vector3Int cellMouse = GameController.INSTANCE.tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            var tileClicked = GameController.INSTANCE.tilemap.GetTile(cellMouse);

            if (tileClicked != null && tileClicked.GetType() == typeof(WorldTile))
            {
                WorldTile wt = tileClicked as WorldTile;
                if (wt.tileType == TileType.WELL)
                {
                    Debug.Log("Clic sur le puits");
                }
                else
                {
                    Debug.Log("Clic ailleurs, dommage");
                }
            }
        }
    }

    void OnMouseUp()
    {
        UIManager.INSTANCE.UpdateCurrentServant(this);
    }

    public void ExecuteAction(GameObject interactiveTile) {
        // TODO
    }
}
