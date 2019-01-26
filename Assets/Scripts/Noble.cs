using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noble : Character
{
    
    private WishEnum currentWish;
    private float currentWishTime; // Time the Noble has been waiting for the completion of the task

    public GameObject wishGameObject;
    public Sprite wishHunt;
    public Sprite wishHungry;

    public override void Start() {
        base.Start();
        InitWish();
        currentWishTime = 0;
    }
    
    public override void Update()
    {
        base.Update();
        
        //TODO asking for servant²
        if (currentWish == WishEnum.WAIT) {
            // Pick a task
            WishEnum[] availableWishes = new WishEnum[] { WishEnum.HUNGRY, WishEnum.HUNT };
            currentWish = availableWishes[(int) Random.Range(0, availableWishes.Length)];
            UIManager.INSTANCE.AddTask(currentWish);
            switch (currentWish) {
                case WishEnum.HUNGRY: 
                    Debug.Log("Current wish hungry");
                    wishGameObject.GetComponent<SpriteRenderer>().sprite = wishHungry;
                break;
                case WishEnum.HUNT:
                Debug.Log("Current wish hunt");
                    wishGameObject.GetComponent<SpriteRenderer>().sprite = wishHunt;
                break;
            }
            
        } else {
            // Increase the number of frames the noble is currently waiting for 
            currentWishTime += Time.deltaTime;
            if (WishExpired(currentWishTime)){
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
        if (!WishExpired(currentWishTime)) {
            currentWish = WishEnum.WAIT;
            currentWishTime = 0;
            GameOverManager.INSTANCE.GameOverVictory();
        }
    }

    bool WishExpired(float elapsedTime) {
        switch (currentWish){
            case WishEnum.HUNGRY:
                if (currentWishTime > 60) {
                    return true;
                } 
                break;
            case WishEnum.HUNT:
                if (currentWishTime > 120) {
                    return true;
                }
                break;
        }
        return false;
    }

    void InitWish() {
        currentWish = WishEnum.WAIT;
        currentWishTime = 0;
    }
}
