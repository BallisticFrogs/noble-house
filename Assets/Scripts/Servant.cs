using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : Character
{
    private Vector2 direction;

    public GameObject waterBucket;
    public GameObject teaPot;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (Input.GetMouseButtonUp(1))
        {
            getWater();
        }
    }

    void OnMouseUp()
    {
        UIManager.INSTANCE.UpdateCurrentServant(this);
    }

    public void ExecuteAction(GameObject interactiveTile) {
        // TODO
    }

    private void getWater()
    {
        Vector3Int cellMouse = GameController.INSTANCE.tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var tileClicked = GameController.INSTANCE.tilemap.GetTile(cellMouse);
        Vector3Int servantCoords = GameController.INSTANCE.tilemap.WorldToCell(transform.position);
        
        if (tileClicked != null && tileClicked.GetType() == typeof(WorldTile))
        {
            WorldTile wt = tileClicked as WorldTile;
            // Si mon perso est à côté du puits
            if (isServantCloseToTile(servantCoords, cellMouse))
            {
                Debug.Log("PF: tileType" + wt.tileType);
                if (wt.tileType == TileType.WELL)
                {
                    Debug.Log("PF: Le serviteur prend l'eau");
                    this.characObject = HoldableObject.WATER_BUCKET;
                    waterBucket.SetActive(true);
                }
                else if(wt.tileType == TileType.KITCHEN && this.characObject == HoldableObject.WATER_BUCKET)
                {
                    Debug.Log("PF: Le serviteur dépose l'eau et prend le thé");
                    this.characObject = HoldableObject.TEA_POT;
                    waterBucket.SetActive(false);
                    teaPot.SetActive(true);
                }
                else if (wt.tileType == TileType.THRONE && this.characObject == HoldableObject.TEA_POT)
                {
                    Debug.Log("PF: Le serviteur dépose le thé");
                    this.characObject = HoldableObject.NONE;
                    teaPot.SetActive(false);
                }
            }
        }
    }

    public void ExecuteAction(GameObject interactiveTile)
    {
        // TODO
    }

    private bool isServantCloseToTile(Vector3Int servantCoords, Vector3Int cellMouse)
    {
        if (Math.Abs(servantCoords.x - cellMouse.x) < 1.5 && Math.Abs(servantCoords.y - cellMouse.y) < 1.5) {
            return true;
        }
        return false;
    }
}
