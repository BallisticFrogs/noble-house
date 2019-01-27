using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noble : Character
{
    [HideInInspector]
    public HoldableObject currentWish;
    private float currentWishTimeCompletion; // Time the Noble has been waiting for the completion of the task
    private float delayBeforeNewTask;

    public GameObject wishGameObject;
    public GameObject bubbleGameObject;

    public Sprite wishWater;
    public Sprite wishTea;
    public Sprite wishHungry;

    public HoldableObject[] availableWishes = new HoldableObject[] { HoldableObject.COOKED_CHICKEN, HoldableObject.WATER_BUCKET, HoldableObject.TEA_POT };
    public int minDelayBeforeNewTask = 10;
    public int maxDelayBeforeNewTask = 20;

    public override void Start()
    {
        base.Start();
        InitNoble();
    }

    public override void Update()
    {
        base.Update();

        //TODO asking for servant²
        if (currentWish == HoldableObject.NONE)
        {
            if (delayBeforeNewTask > 0)
            {
                delayBeforeNewTask -= Time.deltaTime;
            }
            else
            {
                // Pick a task
                currentWish = availableWishes[(int)Random.Range(0, availableWishes.Length)];
                GameController.INSTANCE.AddActiveTasks(this, currentWish);
                bubbleGameObject.SetActive(true);
                wishGameObject.SetActive(true);
                switch (currentWish)
                {
                    case HoldableObject.COOKED_CHICKEN:
                        Debug.Log("Current wish: hungry");
                        wishGameObject.GetComponent<SpriteRenderer>().sprite = wishHungry;
                        break;
                    case HoldableObject.TEA_POT:
                        Debug.Log("Current wish: tea");
                        wishGameObject.GetComponent<SpriteRenderer>().sprite = wishTea;
                        break;
                    case HoldableObject.WATER_BUCKET:
                        Debug.Log("Current wish: water");
                        wishGameObject.GetComponent<SpriteRenderer>().sprite = wishWater;
                        break;
                }
            }

        }
        else
        {
            // Increase the number of frames the noble is currently waiting for 
            currentWishTimeCompletion += Time.deltaTime;
            if (WishExpired(currentWishTimeCompletion))
            {
                GameController.INSTANCE.FailedActiveTask(this);
                // See later for deafeat condition
                // GameOverManager.INSTANCE.GameOverDefeat();
            }
        }
        // TODO replace by true fullfill condition
        if (Input.GetKeyUp(KeyCode.A))
        {
            GameController.INSTANCE.CompleteActiveTask(this);
        }
        // TODO replace by true failed condition
        if (Input.GetKeyUp(KeyCode.Z))
        {
            GameController.INSTANCE.FailedActiveTask(this);
        }
    }

    void UpdateWish(HoldableObject wish)
    {
        currentWish = wish;
        //Do not add task if noble is idling.
        if (HoldableObject.NONE != wish)
        {
            UIManager.INSTANCE.AddTask(currentWish);
        }
    }

    public void FulfillWish()
    {
        if (!WishExpired(currentWishTimeCompletion))
        {
            InitNoble();
            GameController.INSTANCE.CompleteActiveTask(this);
            // GameOverManager.INSTANCE.GameOverVictory();
        }
    }

    public void FailWish()
    {
        InitNoble();
        GameController.INSTANCE.FailedActiveTask(this);
        // GameOverManager.INSTANCE.GameOverVictory();
    }

    bool WishExpired(float elapsedTime)
    {
        switch (currentWish)
        {
            case HoldableObject.COOKED_CHICKEN:
                if (currentWishTimeCompletion > 120)
                {
                    return true;
                }
                break;
            case HoldableObject.WATER_BUCKET:
                if (currentWishTimeCompletion > 60)
                {
                    return true;
                }
                break;
            case HoldableObject.TEA_POT:
                if (currentWishTimeCompletion > 100)
                {
                    return true;
                }
                break;
        }
        return false;
    }

    void InitNoble()
    {
        currentWish = HoldableObject.NONE;
        currentWishTimeCompletion = 0;
        delayBeforeNewTask = Random.Range(minDelayBeforeNewTask, maxDelayBeforeNewTask);
        wishGameObject.SetActive(false);
        bubbleGameObject.SetActive(false);
    }

    public void OrderToKillServant()
    {
        // find closest guard
        GameObject[] guards = GameObject.FindGameObjectsWithTag(Tags.GUARD);
        GameObject closestGuard = FindClosest(guards);

        // find closest servant
        GameObject[] servants = GameObject.FindGameObjectsWithTag(Tags.SERVANT);
        GameObject closestServant = FindClosest(servants);

        // ask guard to kill servant
        if (closestServant != null && closestGuard != null)
        {
            Guard guard = closestGuard.GetComponent<Guard>();
            Servant servant = closestServant.GetComponent<Servant>();
            guard.PleaseKill(servant);
        }
    }

    private GameObject FindClosest(GameObject[] objs)
    {
        GameObject closest = null;
        float dist = float.MaxValue;

        foreach (var currObj in objs)
        {
            float d = (currObj.transform.position - transform.position).magnitude;
            if (d <= dist)
            {
                dist = d;
                closest = currObj;
            }
        }

        return closest;
    }

}
