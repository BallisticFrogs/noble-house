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

    // 9 displayed tasks only. Return false if tasklist full
    public bool AddTask(HoldableObject wish) {
        for (int i = 0 ; i < TASK_LIST_SIZE; i ++) {
            Text taskText = tasksList[i].GetComponent<Text>();
            taskText.text = GetTextFromWish(wish);
            // Do not continue.
            return true;
        }
        return false;
    }

    private string GetTextFromWish(HoldableObject wish){
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
