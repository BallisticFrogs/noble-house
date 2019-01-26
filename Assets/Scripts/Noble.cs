using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noble : Character
{
    
    private WishEnum currentWish;
    private float currentWishTimeCompletion; // Time the Noble has been waiting for the completion of the task
    private float delayBeforeNewTask;

    public GameObject wishGameObject;
    public Sprite wishHunt;
    public Sprite wishHungry;

    public WishEnum[] availableWishes = new WishEnum[] { WishEnum.HUNGRY, WishEnum.THIRSTY };
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
                UIManager.INSTANCE.AddTask(currentWish);
                switch (currentWish) {
                    case WishEnum.HUNGRY: 
                        Debug.Log("Current wish: hungry");
                        wishGameObject.GetComponent<SpriteRenderer>().sprite = wishHungry;
                    break;
                    case WishEnum.THIRSTY:
                    Debug.Log("Current wish: thirsty");
                        wishGameObject.GetComponent<SpriteRenderer>().sprite = wishHunt;
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
            case WishEnum.THIRSTY:
                if (currentWishTimeCompletion > 60) {
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
    }
}
