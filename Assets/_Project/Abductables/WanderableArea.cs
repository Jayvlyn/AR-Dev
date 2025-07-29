using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

public class WanderableArea : MonoBehaviour
{
    public static WanderableArea activeWanderableArea;
    [HideInInspector] public List<Collider> colliders = new();

    private void Awake()
    {
        activeWanderableArea = this;
    }

    private void Update()
    {
        //GameManager.instance.text.text = GameManager.instance.scanning.ToString();
    }

    [SerializeField] private ARPlaneManager arPlaneManager;
    public void SetNewPlanes()
    {
        if (!GameManager.instance.scanning) return;

        colliders.Clear();
        foreach (var trackable in arPlaneManager.trackables)
        {
            if (trackable.TryGetComponent(out Collider collider))
            {
                colliders.Add(collider);
            }
        }
    }

    private int maxLoops = 50;
    public Vector3 GetRandomPosition(int planeIndex)
    {
        Collider area = colliders[planeIndex];
        Bounds bounds = area.bounds;

        int loopCount = 0;
        Vector3 randomPoint;
        do
        {
            loopCount++;
            randomPoint = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        } while (!(IsValidPoint(randomPoint) || loopCount >= maxLoops));

        return area.ClosestPoint(randomPoint);
    }

    [SerializeField] private float distanceFromPlayer = 0.5f;
    private bool IsValidPoint(Vector3 point)
    {
        point.y = 0;
        return point.magnitude > distanceFromPlayer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Vector3.up, distanceFromPlayer);
    }
}