using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Servant currentServant;
    public static GameObject INSTANCE;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateCurrentServant (Servant s) {
        this.currentServant = s;
    }

    void ExecuteAction(GameObject interactiveTile) {
        this.currentServant.ExecuteAction(interactiveTile);
    }
}
