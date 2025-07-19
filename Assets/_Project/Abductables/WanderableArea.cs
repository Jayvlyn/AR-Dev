using UnityEngine;

public class WanderableArea : MonoBehaviour
{
    public static WanderableArea activeWanderableArea;
    [SerializeField] private Collider area;

    private void Awake()
    {
        activeWanderableArea = this;
    }

    public Vector3 GetRandomPosition()
    {
        Bounds bounds = area.bounds;

        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );

        return area.ClosestPoint(randomPoint);
    }
}