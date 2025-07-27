using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockOnController : MonoBehaviour
{
    private LockOnTarget selectedTarget;
    private float distanceFromCamera;
    private Vector2 screenOffsetFromCenter;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void TryLockOnTarget()
    {
        if (selectedTarget != null) return;

        Ray ray = cam.ScreenPointToRay(LockingInputManager.GetMousePosition());
        float rayDistance = Mathf.Infinity;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out LockOnTarget target))
            {
                target.Lock();
                selectedTarget = target;
                distanceFromCamera = hitInfo.distance;

                // Record where on screen it was clicked (offset from center in pixels)
                Vector3 screenPoint = cam.WorldToScreenPoint(hitInfo.point);
                screenOffsetFromCenter = new Vector2(
                    screenPoint.x - Screen.width / 2f,
                    screenPoint.y - Screen.height / 2f
                );
            }
        }
    }

    private void Update()
    {
        if (selectedTarget == null) return;
        PositionLockedTarget();
    }

    private void PositionLockedTarget()
    {
        // Reconstruct the screen position with same offset and distance
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, distanceFromCamera);
        Vector3 targetScreenPos = screenCenter + (Vector3)screenOffsetFromCenter;

        Vector3 worldPos = cam.ScreenToWorldPoint(targetScreenPos);
        selectedTarget.transform.position = worldPos;
    }
}