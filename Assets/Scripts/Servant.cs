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
    public GameObject letter;

    public GameObject poisonedTea;
    public GameObject progressBarBack;
    public GameObject progressBar;

    [HideInInspector]
    public GameObject previousObjectToReactivate;

    public AudioSource fx;

    public override void Start()
    {
        base.Start();
        fx = GetComponent<AudioSource>();
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
                PlayFx(SoundManager.INSTANCE.soundFillBucket);
                break;
            case TileType.LARDER:
                longTask = new LongTask(chicken, HoldableObject.CHICKEN, UnityEngine.Random.Range(1.5f, 3.0f));
                DeactivateAllSprites();
                progressBarBack.SetActive(true);
                PlayFx(SoundManager.INSTANCE.soundBarrel);
                break;
            case TileType.KITCHEN:
                switch (objectInHand)
                {
                    case HoldableObject.WATER_BUCKET:
                        DeactivateAllSprites();
                        longTask = new LongTask(waterBucket, teaPot, HoldableObject.TEA_POT, UnityEngine.Random.Range(2.0f, 5.0f));
                        progressBarBack.SetActive(true);
                        PlayFx(SoundManager.INSTANCE.soundFillBucket);
                        break;
                    case HoldableObject.CHICKEN:
                        DeactivateAllSprites();
                        longTask = new LongTask(chicken, cookedChicken, HoldableObject.COOKED_CHICKEN, UnityEngine.Random.Range(2.0f, 5.0f));
                        progressBarBack.SetActive(true);
                        PlayFx(SoundManager.INSTANCE.soundCook);
                        break;
                }
                break;
            case TileType.POISONED_BUSH:
                if (objectInHand == HoldableObject.WATER_BUCKET)
                {
                    DeactivateAllSprites();
                    longTask = new LongTask(waterBucket, poisonedTea, HoldableObject.POISONED_TEA, UnityEngine.Random.Range(2.0f, 5.0f));
                    progressBarBack.SetActive(true);
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
            case TileType.STABLE:
                switch (objectInHand)
                {
                    case HoldableObject.LETTER:
                        objectInHand = HoldableObject.NONE;
                        objectOwner.FulfillWish();
                        DeactivateAllSprites();
                        PlayFx(SoundManager.INSTANCE.soundHorse);
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
        if (longTask != null && longTask.previousHeldGameObject != null)
        {
            previousObjectToReactivate = longTask.previousHeldGameObject;
        }
        longTask = null;
        if (previousObjectToReactivate)
        {
            previousObjectToReactivate.SetActive(true);
        }
        progressBarBack.SetActive(false);
        progressBar.SetActive(false);
    }

    public void InteractWithNoble(Noble noble)
    {
        if (noble.currentWish == HoldableObject.LETTER)
        {
            DeactivateAllSprites();
            longTask = new LongTask(letter, HoldableObject.LETTER, UnityEngine.Random.Range(1.0f, 2.0f));
            objectOwner = noble;
            progressBarBack.SetActive(true);
        }
        else if (objectInHand == noble.currentWish && noble.currentWish != HoldableObject.LETTER)
        {
            Debug.Log("Je suis content ! Merci");
            DeactivateAllSprites();
            objectInHand = HoldableObject.NONE;
            longTask = null;
            noble.FulfillWish();
        }
        else if (objectInHand == HoldableObject.POISONED_TEA)
        {
            Debug.Log("AaAahHh ! C'est du poison !! Salaud !!");
            DeactivateAllSprites();
            objectInHand = HoldableObject.NONE;
            longTask = null;
            noble.Poison(this);
        }
        else
        {
            Debug.Log("Gardes, débarassez moi de cet incompétent !");
            DeactivateAllSprites();
            objectInHand = HoldableObject.NONE;
            noble.FailWish();
            longTask = null;
        }
    }


    public void InteractWithguard(Guard guard)
    {
        if (objectInHand == HoldableObject.COOKED_CHICKEN)
        {
            Debug.Log("Ouh nice ! Allons buter des nobles !");
            DeactivateAllSprites();
            longTask = null;
            guard.Corrupt();
        } 
        
        if (objectInHand == HoldableObject.POISONED_TEA)
        {
            Debug.Log("Oh bitch, I am dying !");
            DeactivateAllSprites();
            longTask = null;
            guard.Poison(this);
        }
    }

    public void PlayFx(AudioClip clip) {
        fx.Stop();
        fx.PlayOneShot(clip);
    }
}
