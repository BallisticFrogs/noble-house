using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : Fighter
{
    // Start is called before the first frame update

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

    }

}
