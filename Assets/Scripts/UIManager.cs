using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager INSTANCE;
    public GameObject[] tasksList;

    private Slider happynessSlider;
    private Slider angrynessSlider;
    private Slider otherSlider;

    public float happynessMax;
    public float angrynessMax;
    public float otherMax;
    public float happynessLevel;
    public float angrynessLevel;
    public float otherLevel;

    public GameObject task0;
    public GameObject task1;
    public GameObject task2;
    public GameObject task3;
    public GameObject task4;
    public GameObject task5;
    public GameObject task6;
    public GameObject task7;

    public readonly int TASK_LIST_SIZE = 8;
    private int taskCounter=0;
    // Start is called before the first frame update
    void Start()
    {   
        INSTANCE = this;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        tasksList = new GameObject[TASK_LIST_SIZE];
        tasksList[0] = task0;
        tasksList[1] = task1;
        tasksList[2] = task2;
        tasksList[3] = task3;
        tasksList[4] = task4;   
        tasksList[5] = task5;
        tasksList[6] = task6;
        tasksList[7] = task7;

        happynessSlider = GameObject.Find("Happiness").GetComponent<Slider>();
        angrynessSlider = GameObject.Find("Angriness").GetComponent<Slider>();
        otherSlider = GameObject.Find("Other").GetComponent<Slider>();

        happynessSlider.maxValue = happynessMax;
        angrynessSlider.maxValue = angrynessMax;
        otherSlider.maxValue = otherMax;
        happynessSlider.minValue = 0;
        angrynessSlider.minValue = 0;
        otherSlider.minValue = 0;

        happynessLevel = happynessMax / 2;
        angrynessLevel = angrynessMax / 2;
        otherLevel = otherMax / 2;
    }

    // Update is called once per frame
    void Update()
    {
        happynessSlider.value = happynessLevel;
        angrynessSlider.value = angrynessLevel;
        otherSlider.value = otherLevel;
    }



    public void updateListItem(GameObject task, HoldableObject wish) {
        Debug.Log("Update Task " + task + " wish " + wish);
        if ( task == task7 ) {
            task.GetComponent<Text>().text = "And more ...";
        }
        task.GetComponent<Text>().text = GetTextFromWish(wish);
    }


    public string GetTextFromWish(HoldableObject wish){
        switch(wish) 
        {
            case HoldableObject.COOKED_CHICKEN:
            return "I'm hungry!";
            case HoldableObject.WATER_BUCKET:
            return "I'm thirsty for water!";
            case HoldableObject.TEA_POT:
            return "I'm thirsty for tea!";
            default :
            return null;
        }
    }
}
