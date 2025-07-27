using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderingMovement : MonoBehaviour
{
    public enum State
    {
        IDLE, WALKING, EATING, BEING_ABDUCTED, FALLING
    }
    public State currentState;

    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float abductSpeed;
    [SerializeField] private float tolerance;
    [SerializeField, MinMaxSlider(0f, 30)] private Vector2 destinationChangeDelay;
    private float timeOfDestinationChange;
    private bool reachedDestination;
    [SerializeField] Animator animator;
    [SerializeField] Transform pivot;
    private float timeOfEat;
    

    public void GoUpwards()
    {
        ChangeState(State.BEING_ABDUCTED);
    }
    public void StopGoUpwards()
    {
        ChangeState(State.FALLING);
    }

    private void Start()
    {
        ChangeState(State.WALKING);
    }

    private void Update()
    {
        ProcessState();
    }

    private void ApproachDestination()
    {
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
        if (reachedDestination)
        {
            timeOfDestinationChange = Time.time + Random.Range(destinationChangeDelay.x, destinationChangeDelay.y);
            ChangeState(State.IDLE);
        }
    }

    private void AttemptChangeDestination()
    {
        if (Time.time > timeOfDestinationChange)
        {
            ChangeState(State.WALKING);
        }
        else
        {
            if(currentState == State.IDLE)
            {
                if(Random.Range(0,1000) == 0)
                {
                    ChangeState(State.EATING);
                }
            }
        }
    }

    private void ChangeState(State newState)
    {
		switch (currentState) // exit
		{
			case State.IDLE:
				break;
			case State.WALKING:
				SetTrigger("StopWalk");
				break;
			case State.EATING:
				break;
			case State.BEING_ABDUCTED:
                rotSpeed = 0;
                break;
			case State.FALLING:
				break;
		}

		currentState = newState;

		switch (newState)
		{
			case State.IDLE:
				break;
			case State.WALKING:
				targetPosition = WanderableArea.activeWanderableArea.GetRandomPosition();
				reachedDestination = false;
				SetTrigger("EnterWalk");
				break;
			case State.EATING:
				SetTrigger("Eat");
                timeOfEat = Time.time;
				break;
			case State.BEING_ABDUCTED:
                break;
			case State.FALLING:
				break;
		}
	}

    private void ProcessState()
    {
		switch (currentState)
		{
			case State.IDLE:
				AttemptChangeDestination();
				break;
			case State.WALKING:
				ApproachDestination();
				CheckReachDestination();
				break;
			case State.EATING:
                if(Time.time > timeOfEat + 3)
                {
                    ChangeState(State.IDLE);
                }
				break;
			case State.BEING_ABDUCTED:
				transform.Translate(Vector3.up * abductSpeed * Time.deltaTime, Space.World);
                transform.RotateAround(pivot.position, new Vector3(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), rotSpeed);
                rotSpeed += Time.deltaTime * 1.5f;


				break;
			case State.FALLING:
				break;
		}
	}

    float rotSpeed = 0;


	string currentTrigger;
	void SetTrigger(string triggerName)
	{
		if (!string.IsNullOrEmpty(currentTrigger))
			animator.ResetTrigger(currentTrigger); // Clear the previous trigger

		animator.SetTrigger(triggerName); // Set the new trigger
		currentTrigger = triggerName; // Remember the new one
	}
}