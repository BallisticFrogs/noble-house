using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : Fighter
{

    public float jumpHeight = 0.2f;

    private void Jump()
    {
        // transform.position.Set(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
    }

    public override void Update()
    {
        base.Update();

        if (AngryCrowdManager.INSTANCE.rampaging && currentTarget == null)
        {
            SelectNewTarget();
        }
    }

    protected void SelectNewTarget()
    {
        GameObject[] nobles = GameObject.FindGameObjectsWithTag(Tags.NOBLE);
        GameObject[] guards = GameObject.FindGameObjectsWithTag(Tags.GUARD);
        GameObject[] servants = GameObject.FindGameObjectsWithTag(Tags.SERVANT);

        List<GameObject> potentialTargets = new List<GameObject>();
        if (nobles != null && nobles.Length > 0) potentialTargets.AddRange(nobles);
        if (guards != null && guards.Length > 0) potentialTargets.AddRange(guards);
        if (servants != null && servants.Length > 0) potentialTargets.AddRange(servants);

        if (potentialTargets.Count > 0)
        {
            currentTarget = potentialTargets[Random.Range(0, potentialTargets.Count)].GetComponent<Character>();
        }
    }

}
