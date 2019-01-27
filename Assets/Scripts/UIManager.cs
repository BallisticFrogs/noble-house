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

    private static readonly int TASK_LIST_SIZE = 8;
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

    // 9 displayed tasks only. Return false if tasklist full
    public bool AddTask(WishEnum wish) { 
        if (taskCounter < TASK_LIST_SIZE -1) {
            Text taskText = tasksList[taskCounter].GetComponent<Text>();
            taskText.text = GetTextFromWish(wish);
            taskCounter ++;
            Debug.Log("add task " + taskText.text);
            return true;
        }
        return false;
    }

    
    public bool RemoveTask(WishEnum wish) {
        for (int i = 0 ; i < TASK_LIST_SIZE; i++) {
            Text taskText = tasksList[i].GetComponent<Text>();
            if (taskText.text.Equals(GetTextFromWish(wish)) ) {
                taskText.text = "";
                taskCounter --;
                return true;
            }
        }
        return false;
    }

    private string GetTextFromWish(WishEnum wish){
        switch(wish) 
        {
            case WishEnum.HUNGRY:
            return "I'm hungry!";
            case WishEnum.WATER:
            return "I'm thirsty for water!";
            case WishEnum.TEA:
            return "I'm thirsty for tea!";
            default :
            return null;
        }
    }

    // public void addHappiness(){
    //     happynessLevel = Math.Min(happynessMax, happynessLevel ++);
    //     Debug.Log("happyness " + happynessLevel);
    // }

    // public void addAngryness(){
    //     angrynessLevel = Math.Min(angrynessLevel, angrynessLevel ++);
    //     Debug.Log("angryness " + angrynessLevel);
    // }

    // public void addOther(){
    //     otherLevel = Math.Min(otherLevel, otherLevel ++);
    //     Debug.Log("other " + otherLevel);
    // }

    // public void removeHappiness(){
    //     happynessLevel = Math.Max(0, happynessLevel --);
    //     Debug.Log("happyness " + happynessLevel);
    // }

    // public void removeAngryness(){
    //     angrynessLevel = Math.Max(0, angrynessLevel --);
    //     Debug.Log("angryness " + angrynessLevel);
    // }

    // public void removeOther(){
    //     otherLevel = Math.Max(0, otherLevel --);
    //     Debug.Log("other " + otherLevel);
    // }

}
