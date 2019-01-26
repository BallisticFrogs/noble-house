using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryCrowdManager : MonoBehaviour
{
    public static AngryCrowdManager INSTANCE;
    // Start is called before the first frame update
    private int inactivePeasantCounter;
    void Start()
    {
        INSTANCE = this;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        inactivePeasantCounter = transform.childCount;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPeasant() {
        if (inactivePeasantCounter > 0 ) {
            // Look for inactive peasant
            Transform transformChild = findRandomChild(false);
            transformChild.gameObject.SetActive(true);
        }
    }

    public void RemovePeasant() {
        if ( inactivePeasantCounter <= 0 ) {
            Transform transformChild = findRandomChild(true);
            transformChild.gameObject.SetActive(false);
        }
    }

    private Transform findRandomChild(bool active){
        Transform transformChild = transform.GetChild(Random.Range(0, transform.childCount-1));
        if (active == transformChild.gameObject.activeSelf) {
            inactivePeasantCounter --;
            return transformChild;
        } else {
            return findRandomChild(active);
        }
    }


}
