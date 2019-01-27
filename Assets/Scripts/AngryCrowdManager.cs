using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryCrowdManager : MonoBehaviour
{
    public static AngryCrowdManager INSTANCE;

    public int activePeasantBySuccess = 5;

    public bool rampaging;

    [HideInInspector]
    public Transform peasantsStagingArea;

    void Awake()
    {
        INSTANCE = this;
        peasantsStagingArea = GameObject.FindGameObjectWithTag(Tags.PEASANTS_STAGING_AREA).transform;
    }

    public void addPeasants()
    {
        ActivateRandomPeasants(true, activePeasantBySuccess);
    }

    public void ActivateRandomPeasants(bool active, int count)
    {
        List<Transform> children = FindRandomChildren(!active, count);
        foreach (var item in children)
        {
            item.gameObject.SetActive(active);
        }

        int inactivePeasants = CountActiveChildren(false);
        if (inactivePeasants == 0)
        {
            rampaging = true;
        }
    }

    public Peasant FindRandomPeasant()
    {
        List<Transform> result = FindRandomChildren(true, 1);
        if (result != null && result.Count > 0)
        {
            return result[0].GetComponent<Peasant>();
        }
        else
        {
            return null;
        }
    }

    private List<Transform> FindRandomChildren(bool active, int count)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform currChild in peasantsStagingArea.transform)
        {
            if (currChild.gameObject.activeInHierarchy == active)
            {
                children.Add(currChild);
            }
        }

        // shuffle
        children.Sort((a, b) => Random.Range(-1, 1));

        // select some
        List<Transform> result = children.GetRange(0, Mathf.Min(count, children.Count));
        return result;
    }

    private int CountActiveChildren(bool active)
    {
        int count = 0;
        foreach (Transform currChild in peasantsStagingArea.transform)
        {
            if (currChild.gameObject.activeInHierarchy == active)
            {
                count++;
            }
        }
        return count;
    }

}
