using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noble : Character
{
    
    private WishEnum currentWish;
    private float currentWishTime; // Number of frames the Noble is waiting for the completion of his task

    void Start() {
        InitWish();
        currentWishTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        //TODO asking for servant²
        if (currentWish == WishEnum.WAIT) {
            // Pick a task
            WishEnum[] availableWishes = new WishEnum[] { WishEnum.HUNGRY, WishEnum.HUNT };
            currentWish = availableWishes[(int) Random.Range(0, availableWishes.Length)];
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
