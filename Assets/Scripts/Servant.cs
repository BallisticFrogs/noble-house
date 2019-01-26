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

    public void handleClicOnSpecialTile(WorldTile wt)
    {
        switch (wt.tileType)
        {
            case TileType.WELL:
                longTask = new LongTask(waterBucket, HoldableObject.WATER_BUCKET, UnityEngine.Random.Range(2.0f, 4.0f));
                deactivateAllSprites();
                progressBarBack.SetActive(true);
                break;
            case TileType.LARDER:
                longTask = new LongTask(chicken, HoldableObject.CHICKEN, UnityEngine.Random.Range(1.5f, 3.0f));
                deactivateAllSprites();
                progressBarBack.SetActive(true);
                break;
            case TileType.KITCHEN:
                switch (objectInHand)
                {
                    case HoldableObject.WATER_BUCKET:
                        deactivateAllSprites();
                        longTask = new LongTask(waterBucket, teaPot, HoldableObject.TEA_POT, UnityEngine.Random.Range(2.0f, 5.0f));
                        progressBarBack.SetActive(true);
                        break;
                    case HoldableObject.CHICKEN:
                        deactivateAllSprites();
                        longTask = new LongTask(chicken, cookedChicken, HoldableObject.COOKED_CHICKEN, UnityEngine.Random.Range(2.0f, 5.0f));
                        progressBarBack.SetActive(true);
                        break;
                }
                break;
            case TileType.THRONE:
                switch (objectInHand)
                {
                    case HoldableObject.TEA_POT:
                        objectInHand = HoldableObject.NONE;
                        deactivateAllSprites();
                        break;
                    case HoldableObject.COOKED_CHICKEN:
                        objectInHand = HoldableObject.NONE;
                        deactivateAllSprites();
                        break;
                }
                break;
        }
    }

    public void deactivateAllSprites()
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

}
