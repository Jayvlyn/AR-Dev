using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class UFOController : MonoBehaviour
{
    [SerializeField] private FlightMovementController flightMovementController;
    [SerializeField] private AbductorController abductorController;
    [SerializeField] private float attackCycleTime;
    private float timeOfLastAttack;
    [SerializeField] private float hoveringTolerance;
    private bool attacking;
    private bool canStartBeam;
    private bool allowedToAttack = true;

    [SerializeField] private XRGrabInteractable grabber;

    [SerializeField] private GameObject explosionParticles;

    private void Start()
    {
        //grabber.attachTransform = MainAttachPoint.mainAttachPoint;
    }

    public void StopAttacking()
    {
        allowedToAttack = false;
        abductorController.LoseTarget();
        beam.Hide();
    }

    public void AllowAttacking()
    {
        allowedToAttack = true;
    }

    private void Update()
    {
        if (!allowedToAttack) return;
        AttemptAttack();
        AttemptStartBeam();
        AttemptCompleteExtraction();

        AttemptShrinkAbductable();
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

    private void AttemptShrinkAbductable()
    {
        if (attacking)
        {
            float height = abductorController.abductor.GetHeightAboveTarget();
            float maxHeight = abductorController.abductor.startingDist;
            float progress = 1 - Mathf.Clamp01(height / maxHeight);

            if (progress > 0.8f)
            {
                float startScale = abductorController.abductor.activeTargetStartScale;
                float endScale = abductorController.abductor.activeTargetStartScale * 0.1f;

                float scaleProgress = Mathf.InverseLerp(0.8f, 1, progress);
                float targetScale = Mathf.Lerp(startScale, endScale, scaleProgress);

                abductorController.abductor.activeTarget.transform.localScale = Vector3.one * targetScale;
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
        flightMovementController.GoIdle();
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
        GameObject spawned = Instantiate(explosionParticles, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}