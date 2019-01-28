using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager INSTANCE;
    public GameObject[] tasksList;

    public GameObject task0;
    public GameObject task1;
    public GameObject task2;
    public GameObject task3;
    public GameObject task4;
    public GameObject task5;
    public GameObject task6;
    public GameObject task7;
    public GameObject task8;

    private readonly int TASK_LIST_SIZE = 9;

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
        tasksList[8] = task8;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateListItem(GameObject[] nobles) {
        int taskIndex = 0;
        for (int i = 0; i < nobles.Length; i++) {
            Noble noble = nobles[i].GetComponent<Noble>();
            if (!noble.currentWish.Equals(HoldableObject.NONE)) {
                Text taskText = tasksList[taskIndex].GetComponent<Text>();
                if ( taskIndex == TASK_LIST_SIZE-1) {
                    taskText.text = "And more ...";
                    return;
                }
                taskText.text = GetTextFromWish(noble.currentWish);
                taskIndex++;
            }
        }
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
            case HoldableObject.LETTER:
            return "I need to send a letter!";
            default :
            return null;
        }
    }

    public void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
