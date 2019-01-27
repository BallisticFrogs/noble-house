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

    public override void Start() {
        base.Start();
        InitNoble();
    }
    
    public override void Update()
    {
        base.Update();
        
        //TODO asking for servant²
        if (currentWish == HoldableObject.NONE) {
            if (delayBeforeNewTask  > 0) {
                delayBeforeNewTask -= Time.deltaTime;
            } else {
                // Pick a task
                currentWish = availableWishes[(int) Random.Range(0, availableWishes.Length)];
                UIManager.INSTANCE.AddTask(currentWish);
                bubbleGameObject.SetActive(true);
                wishGameObject.SetActive(true); 
                switch (currentWish) {
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
            
        } else {
            // Increase the number of frames the noble is currently waiting for 
            currentWishTimeCompletion += Time.deltaTime;
            if (WishExpired(currentWishTimeCompletion)){
                GameOverManager.INSTANCE.GameOverDefeat();
            }
        }
    }

    void UpdateWish(HoldableObject wish) {
        currentWish = wish;
        //Do not add task if noble is idling.
        if (HoldableObject.NONE != wish ) {
            UIManager.INSTANCE.AddTask(currentWish);
        }      
    }

    public void FulfillWish (){
        if (!WishExpired(currentWishTimeCompletion)) {
            InitNoble();
            // GameOverManager.INSTANCE.GameOverVictory();
        }
    }

    bool WishExpired(float elapsedTime) {
        switch (currentWish){
            case HoldableObject.COOKED_CHICKEN:
                if (currentWishTimeCompletion > 120) {
                    return true;
                } 
                break;
            case HoldableObject.WATER_BUCKET:
                if (currentWishTimeCompletion > 60) {
                    return true;
                }
                break;
            case HoldableObject.TEA_POT:
                if (currentWishTimeCompletion > 100) {
                    return true;
                }
                break;
        }
        return false;
    }

    void InitNoble() {
        currentWish = HoldableObject.NONE;
        currentWishTimeCompletion = 0;
        delayBeforeNewTask = Random.Range(minDelayBeforeNewTask, maxDelayBeforeNewTask);
        wishGameObject.SetActive(false);
        bubbleGameObject.SetActive(false);
    }
}
