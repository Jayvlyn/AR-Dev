using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class FlightMovementController : MonoBehaviour
{
    private BaseFlightMovement activeMovementState;
    [SerializeField] private IdleFlight idleFlight;
    [SerializeField] private TargetedFlight targetedFlight;

    private void Start()
    {
        ChangeState(idleFlight);
    }

    private void ChangeState(BaseFlightMovement newMovementState)
    {
        if(activeMovementState != null) activeMovementState.Exit();
        activeMovementState = newMovementState;
        if(activeMovementState != null) activeMovementState.Enter(this);
    }

    private void Update()
    {
        if(activeMovementState != null) activeMovementState.Update();
    }

    public Transform Target {get; private set;}
    public void TargetFound(Transform _target)
    {
        Target = _target;
        ChangeState(targetedFlight);
    }

    public void GoIdle()
    {
        ChangeState(idleFlight);
    }
}

[System.Serializable]
public abstract class BaseFlightMovement
{
    protected FlightMovementController flightMovementController;
    public void Enter(FlightMovementController controller)
    {
        flightMovementController = controller;
        OnEnter();
    }

    public void Update()
    {
        OnUpdate();
    }

    public void Exit()
    {
        OnExit();
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}

[System.Serializable]
public class IdleFlight : BaseFlightMovement
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float thrustForce;
    private int rotationDirectionMultiplier;
    public override void OnEnter()
    {
        //Set rotation multipler to -1 or 1
        rotationDirectionMultiplier = UnityEngine.Random.Range(0, 2) > 0 ? 1 : -1;
    }

    public override void OnUpdate()
    {
        Vector3 direction = -flightMovementController.transform.position;
        direction = new Vector3(direction.z, 0f, -direction.x);
        rigidbody.AddForce(direction * (thrustForce * rotationDirectionMultiplier));
    }

    public override void OnExit()
    {
        
    }
}

[System.Serializable]
public class TargetedFlight : BaseFlightMovement
{
    private Transform target;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float thrustForce;
    public override void OnEnter()
    {
        target = flightMovementController.Target;
        Vector3 direction = target.position - flightMovementController.transform.position;
        direction = new Vector3(direction.z, 0f, -direction.x);
        rigidbody.AddForce(direction * (thrustForce * 40));
    }

    public override void OnUpdate()
    {
        Vector3 direction = target.position - flightMovementController.transform.position;
        direction.y = 0;
        rigidbody.AddForce(direction * thrustForce);
    }

    public override void OnExit()
    {
        
    }
}