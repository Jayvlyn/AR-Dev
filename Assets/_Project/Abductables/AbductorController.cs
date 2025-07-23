using UnityEngine;

public class AbductorController : MonoBehaviour
{
    [SerializeField] private Abductor abductor;

    public void Abduct()
    {
        abductor.TryStartAbductTarget();
    }

    public bool HasFoundTarget()
    {
        return abductor.HasTarget();
    }

    public Transform GetTarget()
    {
        abductor.TryGetTargetLocation(out var t);
        return t;
    }

    public void FinalizeAbduction()
    {
        abductor.TryStopAbductTarget(true);
    }
    public void LoseTarget()
    {
        abductor.TryStopAbductTarget(false);
    }
}