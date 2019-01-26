using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : MonoBehaviour
{
    // Start is called before the first frame update

    public float jumpHeight;
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Jump() {
        transform.position.Set(transform.position.x, transform.position.y + jumpHeight , transform.position.z);
    }
}
