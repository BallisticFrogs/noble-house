using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Fighter
{

    [HideInInspector]
    public bool corrupted;
    private float timeBeforeCorrupted;

    private Vector3Int postingCellCoords;

    public AudioSource fx;

    public override void Start()
    {
        base.Start();

        // save guarded position
        postingCellCoords = target;
        fx = GetComponent<AudioSource>();
    }

    public override void Update()
    {
        base.Update();

        if (timeBeforeCorrupted > 0)
        {
            timeBeforeCorrupted -= Time.deltaTime;
            if (timeBeforeCorrupted <= 0)
            {
                corrupted = true;
                GameController.INSTANCE.internalPurge = true;
            }
        }

        // handle rampage time : charge !!!
        if (AngryCrowdManager.INSTANCE.rampaging && currentTarget == null)
        {
            currentTarget = AngryCrowdManager.INSTANCE.FindRandomPeasant();
        }

        // handle rebellion/internal purge
        if (GameController.INSTANCE.internalPurge && corrupted && (currentTarget == null || currentTarget.GetType() == typeof(Servant)))
        {
            SelectTargetWhenCorrupt();
        }
        if (GameController.INSTANCE.internalPurge && !corrupted && currentTarget == null)
        {
            SelectTargetWhenNotCorrupt();
        }

        // if no target and not in position, go back there
        if (currentTarget == null && !poisoned && postingCellCoords != GameController.INSTANCE.tilemap.WorldToCell(transform.position))
        {
            SetTarget(postingCellCoords);
        }
    }

    private void SelectTargetWhenCorrupt()
    {
        // try to attck nobles first
        GameObject[] nobles = GameObject.FindGameObjectsWithTag(Tags.NOBLE);
        GameObject closestNoble = FindClosest(nobles);
        if (closestNoble != null)
        {
            currentTarget = closestNoble.GetComponent<Character>();
        }
        else
        {
            // esle attack not corrupted guards
            GameObject[] guards = GameObject.FindGameObjectsWithTag(Tags.GUARD);
            GameObject closestGuard = FindClosest(guards, obj => !obj.GetComponent<Guard>().corrupted);
            if (closestGuard != null)
            {
                currentTarget = closestGuard.GetComponent<Character>();
            }
        }
    }

    private void SelectTargetWhenNotCorrupt()
    {
        // first attack corrupted guards
        GameObject[] guards = GameObject.FindGameObjectsWithTag(Tags.GUARD);
        GameObject closestCorruptGuard = FindClosest(guards, obj => { return obj.GetComponent<Guard>().corrupted; });
        if (closestCorruptGuard != null)
        {
            currentTarget = closestCorruptGuard.GetComponent<Character>();
        }
        else
        {
            // then servants
            GameObject[] servants = GameObject.FindGameObjectsWithTag(Tags.SERVANT);
            GameObject closestServant = FindClosest(servants);
            if (closestServant != null)
            {
                currentTarget = closestServant.GetComponent<Character>();
            }
        }
    }

    protected override void OnPoisonHit(int hitCount, Servant poisonMurderer)
    {
        if (hitCount == 2 || hitCount == 4)
        {
            OrderToKillSpecificServant(poisonMurderer);
        }
    }

    public void OrderToKillSpecificServant(Servant servant)
    {
        // find closest guard
        GameObject[] guards = GameObject.FindGameObjectsWithTag(Tags.GUARD);
        GameObject closestGuard = FindClosest(guards);
        if (servant != null && closestGuard != null)
        {
            Guard guard = closestGuard.GetComponent<Guard>();
            guard.PleaseKill(servant);
        }
    }

    public void Corrupt()
    {
        timeBeforeCorrupted = GameController.INSTANCE.guardCorruptionDelay;

    }
    
    public void PlayFx(AudioClip clip) {
        fx.PlayOneShot(clip);
    }

}
