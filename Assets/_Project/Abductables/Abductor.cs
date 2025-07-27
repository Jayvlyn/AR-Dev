using System;
using UnityEngine;

public class Abductor : MonoBehaviour
{
    [SerializeField, Min(0)] private float checkForTargetDelay = 1f;
    private float timeOfLastTargetCycleCheck;

    [HideInInspector] public Abducatable activeTarget;
    [HideInInspector] public float startingDist;
    [HideInInspector] public float activeTargetStartScale;

    private void Update()
    {
        PrepareTarget();
    }
    
    public bool HasTarget() => activeTarget != null;

    private void FindAbductionTarget()
    {
        if (AbductionController.TryGetFreeAbductable(out var abductable))
        {
            activeTarget = abductable;
            activeTargetStartScale = activeTarget.transform.localScale.x;
        }
    }
    private void PrepareTarget()
    {
        if (HasTarget()) return;
        if (timeOfLastTargetCycleCheck + checkForTargetDelay < Time.time)
        {
            FindAbductionTarget();
            timeOfLastTargetCycleCheck = Time.time;
        }
    }

    public bool TryGetTargetLocation(out Transform location)
    {
        if (!HasTarget())
        {
            location = null;
            return false;
        }
        location = activeTarget.transform;
        return true;
    }

    public bool TryStartAbductTarget()
    {
        if (!HasTarget()) return false;
        activeTarget.StartAbduction(this);
        startingDist = GetHeightAboveTarget();
        return true;
    }

    public void TryStopAbductTarget(bool successful)
    {
        if (!HasTarget()) return;

        if(successful) activeTarget.CompleteAbduction();
        else activeTarget.FailAbduction();
        activeTarget = null;

    }

    public float GetHeightAboveTarget()
    {
        return transform.position.y - activeTarget.transform.position.y;
    }
}