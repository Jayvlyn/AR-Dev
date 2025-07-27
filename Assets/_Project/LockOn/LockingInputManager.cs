using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class LockingInputManager : MonoBehaviour
{
    [SerializeField] private LockOnController lockControl;
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Touch.activeTouches.Count > 0)
        {
            lockControl.TryLockOnTarget();
        }
    }

    public static Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }
}