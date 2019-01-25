using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        UIManager.INSTANCE.UpdateCurrentServant(this);
    }

    void ExecuteAction(GameObject interactiveTile) {
        // TODO
    }
}
