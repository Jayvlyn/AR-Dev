using System;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    [SerializeField] private FlightMovementController flightMovementController;
    [SerializeField] private AbductorController abductorController;
    [SerializeField] private float attackCycleTime;
    private float timeOfLastAttack;
    [SerializeField] private float hoveringTolerance;
    private bool attacking;
    private bool canStartBeam;

    private void Update()
    {
        AttemptAttack();
        AttemptStartBeam();
        AttemptCompleteExtraction();

        if(attacking)
        {
            float height = abductorController.abductor.GetHeightAboveTarget();
            float maxHeight = abductorController.abductor.startingDist;
            float progress = 1 - Mathf.Clamp01(height / maxHeight);

            if (progress > 0.8f)
            {
                float scaleProgress = Mathf.InverseLerp(0.8f, 1, progress);
                float targetScale = Mathf.Lerp(1, 0.1f, scaleProgress);

                abductorController.abductor.activeTarget.transform.localScale = Vector3.one * targetScale;
            }
        }
    }
    
    private void AttemptCompleteExtraction()
    {
        if (!attacking) return;
        float yTarget = abductorController.GetTarget().transform.position.y;
        if (yTarget > transform.position.y)
        {
            CompletedExtraction();
        }
    }

    private void AttemptStartBeam()
    {
        if (attacking || !canStartBeam) return;
        if (abductorController.HasFoundTarget())
        {
            
            Vector3 target = abductorController.GetTarget().transform.position;
            Vector3 selfPos = transform.position;
            target.y = selfPos.y;
            float distanceToTarget = Vector3.Distance(target, selfPos);
            if (distanceToTarget < hoveringTolerance)
            {
                TargetReached();
            }
        }
    }

    [SerializeField] private TractorBeam beam;
    private void TargetReached()
    {
        canStartBeam = false;
        attacking = true;
        beam.Reveal();
        abductorController.Abduct();
    }

    private void CompletedExtraction()
    {
        attacking = false;
        beam.Hide();
        abductorController.FinalizeAbduction();
    }

    

    private void AttemptAttack()
    {
        if (attacking) return;
        if (timeOfLastAttack + attackCycleTime < Time.time)
        {
            if (abductorController.HasFoundTarget())
            {
                flightMovementController.TargetFound(abductorController.GetTarget());
                canStartBeam = true;
            }
            timeOfLastAttack = Time.time;
        }
    }

    public void OnRemoval()
    {
        abductorController.LoseTarget();
    }
}