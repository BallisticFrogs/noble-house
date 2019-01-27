using UnityEngine;

public class Character : Mobile
{
    public int life = 100;

    public HoldableObject objectInHand;

    public LongTask longTask;

    public override void Update()
    {
        base.Update();

        if (life <= 0)
        {
            Die();
        }
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

}


public class LongTask
{
    public GameObject previousHeldGameObject;
    public GameObject gameObjectToActivate;
    public HoldableObject objectInProgress;

    public float timer;
    public float totalTime;

    public LongTask(GameObject gameObjectToActivate, HoldableObject objectInProgress, float timer)
    {
        this.gameObjectToActivate = gameObjectToActivate;
        this.objectInProgress = objectInProgress;
        this.timer = timer;
        this.totalTime = timer;
    }

    public LongTask(GameObject previousHeldGameObject, GameObject gameObjectToActivate, HoldableObject objectInProgress, float timer)
    {
        this.previousHeldGameObject = previousHeldGameObject;
        this.gameObjectToActivate = gameObjectToActivate;
        this.objectInProgress = objectInProgress;
        this.timer = timer;
        this.totalTime = timer;
    }
}