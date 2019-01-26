using UnityEngine;

public class Character : Mobile
{
    public HoldableObject objectInHand;

    public LongTask longTask;
}

public class LongTask
{
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
}