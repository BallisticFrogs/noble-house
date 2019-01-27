using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noble : Character
{
    
    private WishEnum currentWish;
    private float currentWishTimeCompletion; // Time the Noble has been waiting for the completion of the task
    private float delayBeforeNewTask;

    public GameObject wishGameObject;
    public GameObject bubbleGameObject;

    public Sprite wishWater;
    public Sprite wishTea;
    public Sprite wishHungry;

    public WishEnum[] availableWishes = new WishEnum[] { WishEnum.HUNGRY, WishEnum.WATER, WishEnum.TEA };
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
        if (currentWish == WishEnum.WAIT) {
            if (delayBeforeNewTask  > 0) {
                delayBeforeNewTask -= Time.deltaTime;
            } else {
                // Pick a task
                currentWish = availableWishes[(int) Random.Range(0, availableWishes.Length)];
                GameController.INSTANCE.AddActiveTasks(this, currentWish);
                bubbleGameObject.SetActive(true);
                wishGameObject.SetActive(true); 
                switch (currentWish) {
                    case WishEnum.HUNGRY: 
                        Debug.Log("Current wish: hungry");
                        wishGameObject.GetComponent<SpriteRenderer>().sprite = wishHungry;
                    break;
                    case WishEnum.TEA:
                    Debug.Log("Current wish: tea");
                        wishGameObject.GetComponent<SpriteRenderer>().sprite = wishTea;
                    break;
                    case WishEnum.WATER:
                    Debug.Log("Current wish: water");
                        wishGameObject.GetComponent<SpriteRenderer>().sprite = wishWater;
                    break;
                }
            }
            
        } else {
            // Increase the number of frames the noble is currently waiting for 
            currentWishTimeCompletion += Time.deltaTime;
            if (WishExpired(currentWishTimeCompletion)){
                GameController.INSTANCE.FailedActiveTask(this);
                // See later for deafeat condition
                // GameOverManager.INSTANCE.GameOverDefeat();
            }
        }
        // TODO replace by true fullfill condition
        if (Input.GetKeyUp(KeyCode.A)) {
            GameController.INSTANCE.CompleteActiveTask(this);
        }
        // TODO replace by true failed condition
        if (Input.GetKeyUp(KeyCode.Z)) {
            GameController.INSTANCE.FailedActiveTask(this);
        }
    }

    void UpdateWish(WishEnum wish) {
        currentWish = wish;
        //Do not add task if noble is idling.
        if (WishEnum.WAIT != wish ) {
            UIManager.INSTANCE.AddTask(currentWish);
        }      
    }

    void FulfillWish (){
        if (!WishExpired(currentWishTimeCompletion)) {
            InitNoble();
            GameController.INSTANCE.CompleteActiveTask(this);
            // GameOverManager.INSTANCE.GameOverVictory();
        }
    }

    bool WishExpired(float elapsedTime) {
        switch (currentWish){
            case WishEnum.HUNGRY:
                if (currentWishTimeCompletion > 120) {
                    return true;
                } 
                break;
            case WishEnum.WATER:
                if (currentWishTimeCompletion > 60) {
                    return true;
                }
                break;
            case WishEnum.TEA:
                if (currentWishTimeCompletion > 100) {
                    return true;
                }
                break;
        }
        return false;
    }

    void InitNoble() {
        currentWish = WishEnum.WAIT;
        currentWishTimeCompletion = 0;
        delayBeforeNewTask = Random.Range(minDelayBeforeNewTask, maxDelayBeforeNewTask);
        wishGameObject.SetActive(false);
        bubbleGameObject.SetActive(false);
    }
}
