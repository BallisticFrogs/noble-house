using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : Character
{
    private Vector2 direction;
    
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

    public void ExecuteAction(GameObject interactiveTile) {
        // TODO
    }
}
