using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noble : Character
{
    [HideInInspector]
    public HoldableObject currentWish;
    public HoldableObject previousWish;
    private float currentWishTimeCompletion; // Time the Noble has been waiting for the completion of the task
    private float delayBeforeNewTask;

    public GameObject wishGameObject;
    public GameObject bubbleGameObject;
    public GameObject pukePrefab;
    public GameObject crossGameObject;
    
    public Sprite wishWater;
    public Sprite wishTea;
    public Sprite wishHungry;
    private bool dying = false;
    private float hitDelay = 0;
    private int poisonDamage;
    private Servant murderer;
    private int hitCounter = 0;
    public HoldableObject[] availableWishes = new HoldableObject[] { HoldableObject.COOKED_CHICKEN, HoldableObject.WATER_BUCKET, HoldableObject.TEA_POT };
    public int minDelayBeforeNewTask = 2;
    public int maxDelayBeforeNewTask = 5;

    public int cookedChickenCompletionTime = 120;
    public int waterCompletionTime = 40;
    public int teaCompletionTime = 100;

    public int crossBlinkDelay = 2;

    public override void Start()
    {
        base.Start();
        InitNoble();
    }

    public override void Update()
    {
        base.Update();
        if (dying) {
            UpdateDying();
        }
        //TODO asking for servant²
        if (currentWish == HoldableObject.NONE)
        {
            if (delayBeforeNewTask > 0)
            {
                delayBeforeNewTask -= Time.deltaTime;
            } else {
                PickTask();
            }

        }
        else
        {
            // Increase the number of frames the noble is currently waiting for 
            currentWishTimeCompletion += Time.deltaTime;

            // Update the color of the bubble and the visibility of the cross
            UpdateBubble();

            if (WishExpired(currentWishTimeCompletion))
            {
                crossGameObject.SetActive(true);
                GameController.INSTANCE.FailedActiveTask(this);
                // See later for deafeat condition
                // GameOverManager.INSTANCE.GameOverDefeat();
            }
        }
    }

    void PickTask()
    {
        currentWish = availableWishes[(int)Random.Range(0, availableWishes.Length)];
        if (previousWish != HoldableObject.NONE && previousWish == currentWish)
        {
            PickTask();
        }
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

    public void FulfillWish (){
        if (!WishExpired(currentWishTimeCompletion)) {
            previousWish = currentWish;
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
        if (currentWishTimeCompletion > GetExpectedWishCompletionTime(currentWish)) {
            return true;
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
        crossGameObject.SetActive(false);
        bubbleGameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    public void KillNoble(Servant servant) {
        dying = true;
        poisonDamage = Random.Range(20, 100);
        murderer = servant;
    }

    private void UpdateDying(){
        if (hitDelay > 0)
        {
            hitDelay -= Time.deltaTime;
        }
        if (hitDelay <= 0)
        {
            life -= poisonDamage;
            hitDelay = 1;
            hitCounter += 1;
            target = GameController.INSTANCE.GetDyingTarget(this);
            Instantiate(pukePrefab, transform.position, transform.rotation);
            if (hitCounter > 2 ) {
                OrderToKillSpecificServant(murderer);
            }
        }
    }

    public void OrderToKillSpecificServant(Servant servant){
        // find closest guard
        GameObject[] guards = GameObject.FindGameObjectsWithTag(Tags.GUARD);
        GameObject closestGuard = FindClosest(guards);
        if (servant != null && closestGuard != null)
        {
            Guard guard = closestGuard.GetComponent<Guard>();
            guard.PleaseKill(servant);
        }
    }

    public void OrderToKillClosestServant() {
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

    private int GetExpectedWishCompletionTime (HoldableObject o) {
        switch (currentWish)
        {
            case HoldableObject.COOKED_CHICKEN:
                return cookedChickenCompletionTime;
            case HoldableObject.WATER_BUCKET:
                return waterCompletionTime;
            case HoldableObject.TEA_POT:
                return teaCompletionTime;
            default:
                return 0;
        }
    }

    private void UpdateBubble() {
        // Color of the bubble
        float redColor = currentWishTimeCompletion / GetExpectedWishCompletionTime(currentWish);
        if (redColor < 0.25) {
            redColor = 0f;
        }  else if (redColor < 0.5) {
            redColor = 0.25f;
        } else if (redColor < 0.75) {
            redColor = 0.5f;
        } else {
            redColor = 1f;
        }
        bubbleGameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1 - redColor, 1 - redColor);

        // cross state
        if (currentWishTimeCompletion > GetExpectedWishCompletionTime(currentWish) / 2) {
            if (currentWishTimeCompletion % crossBlinkDelay*2 < crossBlinkDelay) {
                crossGameObject.SetActive(true);
            } else {
                crossGameObject.SetActive(false);
            }
        }
    }
}
