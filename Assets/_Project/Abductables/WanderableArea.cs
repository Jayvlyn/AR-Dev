using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

public class WanderableArea : MonoBehaviour
{
    public static WanderableArea activeWanderableArea;
    private List<Collider> colliders = new();

    private void Awake()
    {
        activeWanderableArea = this;
    }

    [SerializeField] private ARPlaneManager arPlaneManager;
    public void SetNewPlanes()
    {
        colliders.Clear();
        foreach (var trackable in arPlaneManager.trackables)
        {
            if (trackable.TryGetComponent(out Collider collider))
            {
                if (collider.transform.rotation.eulerAngles.z == 0)
                {
                    colliders.Add(collider);
                }
            }
        }
    }
    public Vector3 GetRandomPosition()
    {
        return Vector3.zero;
        // Bounds bounds = area.bounds;
        //
        // Vector3 randomPoint = new Vector3(
        //     Random.Range(bounds.min.x, bounds.max.x),
        //     Random.Range(bounds.min.y, bounds.max.y),
        //     Random.Range(bounds.min.z, bounds.max.z)
        // );
        //
        // return area.ClosestPoint(randomPoint);
    }
}