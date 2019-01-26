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
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public void handleClicOnSpecialTile(WorldTile wt)
    {
        // Debug.Log("PF: tileType" + wt.tileType);
        if (wt.tileType == TileType.WELL)
        {
            Debug.Log("PF: Le serviteur prend l'eau");
            this.objectInHand = HoldableObject.WATER_BUCKET;
            waterBucket.SetActive(true);
        }
        else if (wt.tileType == TileType.KITCHEN && this.objectInHand == HoldableObject.WATER_BUCKET)
        {
            Debug.Log("PF: Le serviteur dépose l'eau et prend le thé");
            this.objectInHand = HoldableObject.TEA_POT;
            waterBucket.SetActive(false);
            teaPot.SetActive(true);
        }
        else if (wt.tileType == TileType.THRONE && this.objectInHand == HoldableObject.TEA_POT)
        {
            Debug.Log("PF: Le serviteur dépose le thé");
            this.objectInHand = HoldableObject.NONE;
            teaPot.SetActive(false);
        }
    }

}
