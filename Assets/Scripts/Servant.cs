using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Servant : Character
{
    public Vector3 target;
    public Vector3 waypoint;
    public float speed;

    public Tilemap tileMap;
    private Grid grid;

    private WorldTile currentTile;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        this.grid = tileMap.layoutGrid;    
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        // Clic côté droit
        if (Input.GetMouseButtonUp(1))
        {
            // Coord du clic sur la grille
            Vector3Int cellMouse = GameController.INSTANCE.tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            // La tuile cliquée
            var tileClicked = GameController.INSTANCE.tilemap.GetTile(cellMouse);
            // Les coord de mon perso
            Vector3Int servantCoords = GameController.INSTANCE.tilemap.WorldToCell(transform.position);

            // Si ma tileClicked est un puits
            if (tileClicked != null && tileClicked.GetType() == typeof(WorldTile))
            {
                WorldTile wt = tileClicked as WorldTile;
                // Si mon perso est à côté
                if ((wt.tileType == TileType.WELL) && isServantClosedToTile(servantCoords, cellMouse))
                {
                    Debug.Log("PF: Le serviteur prend l'eau");
                }
            }
        }
    }

    private void OnMouseUp()
    {
        UIManager.INSTANCE.UpdateCurrentServant(this);
    }

    public void ExecuteAction(Resources interactiveObject) {
        this.target = interactiveObject.transform.position;
    }

    private bool isServantClosedToTile(Vector3Int servantCoords, Vector3Int cellMouse)
    {
        if (Math.Abs(servantCoords.x - cellMouse.x) < 1.5 && Math.Abs(servantCoords.y - cellMouse.y) < 1.5) {
            return true;
        }
        return false;
    }
}
