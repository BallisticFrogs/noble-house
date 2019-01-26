using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : Character
{
    private Vector2 direction;

    public GameObject waterBucket;
    public GameObject teaPot;
    public GameObject chicken;
    public GameObject cookedChicken;

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
        Debug.Log(wt);
        switch (wt.tileType)
        {
            case TileType.WELL:
                Debug.Log("PF: Le serviteur prend l'eau");
                this.objectInHand = HoldableObject.WATER_BUCKET;
                waterBucket.SetActive(true);
                break;
            case TileType.LARDER:
                Debug.Log("PF: Le serviteur prend le poulet");
                this.objectInHand = HoldableObject.CHICKEN;
                chicken.SetActive(true);
                break;
            case TileType.KITCHEN:
                switch (this.objectInHand)
                {
                    case HoldableObject.WATER_BUCKET:
                        Debug.Log("PF: Le serviteur dépose l'eau et prend le thé");
                        this.objectInHand = HoldableObject.TEA_POT;
                        waterBucket.SetActive(false);
                        teaPot.SetActive(true);
                        break;
                    case HoldableObject.CHICKEN:
                        Debug.Log("PF: Le serviteur dépose le poulet et le fait cuire");
                        this.objectInHand = HoldableObject.COOKED_CHICKEN;
                        chicken.SetActive(false);
                        cookedChicken.SetActive(true);
                        break;
                }
                break;
            case TileType.THRONE:
                switch (this.objectInHand)
                {
                    case HoldableObject.TEA_POT:
                        Debug.Log("PF: Le serviteur dépose le thé");
                        this.objectInHand = HoldableObject.NONE;
                        teaPot.SetActive(false);
                        break;
                    case HoldableObject.COOKED_CHICKEN:
                        Debug.Log("PF: Le serviteur donne le poulet cuit");
                        this.objectInHand = HoldableObject.NONE;
                        cookedChicken.SetActive(false);
                        break;
                }
                break;
        }
    }

}
