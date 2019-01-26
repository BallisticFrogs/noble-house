using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Servant currentServant;
    public static UIManager INSTANCE;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCurrentServant (Servant s) {
        this.currentServant = s;
    }

    public void ExecuteAction(GameObject interactiveTile) {
        this.currentServant.ExecuteAction(interactiveTile);
    }
}
