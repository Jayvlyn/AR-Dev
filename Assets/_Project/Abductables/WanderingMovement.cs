using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderingMovement : MonoBehaviour
{
    Animator animator;
    public enum MoveState
    {
        IDLE, WALKING, EATING, FLOATING
    }
    public MoveState currentMoveState;

    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float tolerance;
    [SerializeField, MinMaxSlider(0f, 30)] private Vector2 destinationChangeDelay;
    private float timeOfDestinationChange;
    private bool reachedDestination;

    private void Start()
    {
        currentMoveState = MoveState.IDLE;

        targetPosition = WanderableArea.activeWanderableArea.GetRandomPosition();
    }

    private void Update()
    {
        AttemptApproachDestination();
        CheckReachDestination();
        AttemptChangeDestination();
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

    public void ChangeMoveState(MoveState newState)
    {
        // ON EXIT
        switch (currentMoveState)
        {
            case MoveState.IDLE:
                break;
            case MoveState.WALKING:
                SetTrigger("StopWalk");
                break;
            case MoveState.EATING:
                break;
            case MoveState.FLOATING:
                break;
        }


        currentMoveState = newState;

        // ON ENTER
        switch (newState)
        {
            case MoveState.IDLE:
                break;
            case MoveState.WALKING:
                SetTrigger("EnterWalk");
                break;
            case MoveState.EATING:
                SetTrigger("Eat");
                break;
            case MoveState.FLOATING:
                break;
        }
    }

    string currentTrigger;
    void SetTrigger(string triggerName)
    {
        if (!string.IsNullOrEmpty(currentTrigger))
            animator.ResetTrigger(currentTrigger); // Clear the previous trigger

        animator.SetTrigger(triggerName); // Set the new trigger
        currentTrigger = triggerName; // Remember the new one
    }
}