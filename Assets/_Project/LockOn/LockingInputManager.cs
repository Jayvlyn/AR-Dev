using UnityEngine;
using UnityEngine.InputSystem;

public class LockingInputManager : MonoBehaviour
{
    [SerializeField] private LockOnController lockControl;
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            lockControl.TryLockOnTarget();
        }
    }

    public static Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }
}