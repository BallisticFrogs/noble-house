using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noble : MonoBehaviour
{
    
    private WishEnum currentWish = WishEnum.WAIT;


    // Update is called once per frame
    void Update()
    {
        //TODO asking for servant²
    }

    void UpdateWish(WishEnum wish) {
        currentWish = wish;
    }
}
