using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderingMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float tolerance;
    [SerializeField, MinMaxSlider(0f, 30)] private Vector2 destinationChangeDelay;
    private float timeOfDestinationChange;
    private bool reachedDestination;


    private bool inbeam;
    public void GoUpwards()
    {
        inbeam = true;
    }
    public void StopGoUpwards()
    {
        inbeam = false;
    }

    private void Start()
    {
        targetPosition = WanderableArea.activeWanderableArea.GetRandomPosition();
    }

    [SerializeField] private float upSpeed = 0.1f;
    private void Update()
    {
        if (inbeam)
        {
            transform.Translate(Vector3.up * (Time.deltaTime * upSpeed));
        }
        else
        {
            AttemptApproachDestination();
            CheckReachDestination();
            AttemptChangeDestination();
            
        }
    }

    private void AttemptApproachDestination()
    {
        if(reachedDestination) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        RotateTowardsDestination();
    }

    private void RotateTowardsDestination()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void CheckReachDestination()
    {
        if(reachedDestination) return;
        reachedDestination = (Vector3.Distance(transform.position, targetPosition) < tolerance);
        if(reachedDestination) timeOfDestinationChange = Time.time + Random.Range(destinationChangeDelay.x, destinationChangeDelay.y);
    }

    private void AttemptChangeDestination()
    {
        if (!reachedDestination) return;

        if (Time.time > timeOfDestinationChange)
        {
            targetPosition = WanderableArea.activeWanderableArea.GetRandomPosition();
            reachedDestination = false;
        }
    }
}