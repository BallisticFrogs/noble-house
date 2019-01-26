using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    private static readonly int TASK_LIST_SIZE = 9;
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
        for (int i = 0 ; i < TASK_LIST_SIZE ; i++ ) {
            tasksList[i] = GameObject.Find("Task"+i);
        }
        happynessSlider = GameObject.Find("Happiness").GetComponent<Slider>();
        angrynessSlider = GameObject.Find("Angriness").GetComponent<Slider>();
        otherSlider = GameObject.Find("Other").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        happynessSlider.value = happynessLevel;
        angrynessSlider.value = angrynessLevel;
        otherSlider.value = otherLevel;
    }

    // 9 tasks only. Return false if tasklist full
    public bool AddTask(WishEnum wish) {
        for (int i = 0 ; i < TASK_LIST_SIZE; i ++) {
            if (!tasksList[i].activeSelf) {
                tasksList[i].SetActive(true);
                Text taskText = tasksList[i].GetComponent<Text>();
                taskText.text = GetTextFromWish(wish);
                // Do not continue.
                return true;
            }   
        }
        return false;
    }

    private string GetTextFromWish(WishEnum wish){
        switch(wish) 
        {
            case WishEnum.HUNGRY:
            return "I'm hungy!";
            case WishEnum.HUNT:
            return "I want to hung!";
            case WishEnum.STROLL:
            return "I want to go walking!";
            default :
            return null;
        }
    }

}
