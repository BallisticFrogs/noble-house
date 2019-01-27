using System;
using UnityEngine;

public class Character : Mobile
{

    private readonly String poisonPukePrefabPath = "Prefabs/pukeSplash";
    private readonly String bloodSpaslPrefabPath = "Prefabs/BloodSplash";

    public float life = 100;

    public HoldableObject objectInHand;

    public LongTask longTask;

    public Noble objectOwner;


    protected bool poisoned = false;
    private int poisonHitCounter = 0;
    private float poisonHitDelay = 0;
    private int poisonDamage;
    private Servant poisonMurderer;

    public override void Update()
    {
        base.Update();
        if (poisoned)
        {
            UpdatePoisoned();
        }

        if (life <= 0)
        {
            Die();
        }
    }

    public void OnPhysicalHit()
    {
        GameObject pukePrefab = Resources.Load<GameObject>(bloodSpaslPrefabPath);
        Instantiate(pukePrefab, transform.position, transform.rotation);
    }

    private void UpdatePoisoned()
    {
        if (poisonHitDelay > 0)
        {
            poisonHitDelay -= Time.deltaTime;
        }
        if (poisonHitDelay <= 0)
        {
            life -= poisonDamage;
            poisonHitDelay = 1;
            poisonHitCounter += 1;

            target = GameController.INSTANCE.GetAgonizingMoveTarget(this);

            GameObject pukePrefab = Resources.Load<GameObject>(poisonPukePrefabPath);
            Instantiate(pukePrefab, transform.position, transform.rotation);

            OnPoisonHit(poisonHitCounter, poisonMurderer);
        }
    }

    public void Poison(Servant servant)
    {
        poisoned = true;
        poisonDamage = UnityEngine.Random.Range(10, 20);
        poisonMurderer = servant;
        speed = speed * 0.2f;
    }

    protected virtual void OnPoisonHit(int hitCount, Servant poisonMurderer)
    {
        // no op here
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected GameObject FindClosest(GameObject[] objs)
    {
        return FindClosest(objs, null);
    }

    protected GameObject FindClosest(GameObject[] objs, Func<GameObject, bool> filter)
    {
        GameObject closest = null;
        float dist = float.MaxValue;

        foreach (var currObj in objs)
        {
            // skip self
            if (currObj == gameObject) continue;

            if (filter != null && filter.Invoke(currObj))
            {
                continue;
            }

            float d = (currObj.transform.position - transform.position).magnitude;
            if (d <= dist)
            {
                dist = d;
                closest = currObj;
            }
        }

        return closest;
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