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
    public GameObject progressBarBack;
    public GameObject progressBar;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (longTask != null)
        {
            float progress = 1 - (longTask.timer / longTask.totalTime);
            progressBar.SetActive(true);
            progressBar.transform.localScale = new Vector3(progress, 0.79f, 1);

            longTask.timer -= Time.deltaTime;
            if (longTask.timer <= 0)
            {
                progressBarBack.SetActive(false);
                progressBar.SetActive(false);

                objectInHand = longTask.objectInProgress;
                longTask.objectInProgress = HoldableObject.NONE;
                longTask.gameObjectToActivate.SetActive(true);
                longTask = null;
            }
        }
    }

    public void HandleClicOnSpecialTile(WorldTile wt)
    {
        switch (wt.tileType)
        {
            case TileType.WELL:
                longTask = new LongTask(waterBucket, HoldableObject.WATER_BUCKET, UnityEngine.Random.Range(2.0f, 4.0f));
                DeactivateAllSprites();
                progressBarBack.SetActive(true);
                break;
            case TileType.LARDER:
                longTask = new LongTask(chicken, HoldableObject.CHICKEN, UnityEngine.Random.Range(1.5f, 3.0f));
                DeactivateAllSprites();
                progressBarBack.SetActive(true);
                break;
            case TileType.KITCHEN:
                switch (objectInHand)
                {
                    case HoldableObject.WATER_BUCKET:
                        DeactivateAllSprites();
                        longTask = new LongTask(waterBucket, teaPot, HoldableObject.TEA_POT, UnityEngine.Random.Range(2.0f, 5.0f));
                        progressBarBack.SetActive(true);
                        break;
                    case HoldableObject.CHICKEN:
                        DeactivateAllSprites();
                        longTask = new LongTask(chicken, cookedChicken, HoldableObject.COOKED_CHICKEN, UnityEngine.Random.Range(2.0f, 5.0f));
                        progressBarBack.SetActive(true);
                        break;
                }
                break;
            case TileType.THRONE:
                switch (objectInHand)
                {
                    case HoldableObject.NONE:
                        Debug.Log("Pas possible, y'a rien à donner");
                        break;
                    default:
                        objectInHand = HoldableObject.NONE;
                        DeactivateAllSprites();
                        break;
                }
                break;
        }
    }

    public void DeactivateAllSprites()
    {
        for (int sprite = 0; sprite < transform.childCount; sprite++)
        {
            transform.GetChild(sprite).gameObject.SetActive(false);
        }
    }

    public override void SetTarget(Vector3Int newTarget)
    {
        base.SetTarget(newTarget);
        var previousObjectToReactivate = new GameObject();
        if (longTask != null && longTask.previousHeldGameObject != null)
        {
            previousObjectToReactivate = longTask.previousHeldGameObject;
        }
        longTask = null;
        previousObjectToReactivate.SetActive(true);
        progressBarBack.SetActive(false);
        progressBar.SetActive(false);
    }

    public void InteractWithNoble()
    {
        // TODO stuff
        Debug.Log("interact with noble");
    }

}
