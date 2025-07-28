using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class LockingInputManager : MonoBehaviour
{
    // [SerializeField] private LockOnController lockControl;
    // private void Update()
    // {
    //     if (Mouse.current.leftButton.wasPressedThisFrame || Touch.activeTouches.Count > 0)
    //     {
    //         //Touch.onFingerDown += OnFingerDown;
    //         lockControl.TryLockOnTarget();
    //     }
    // }
    //
    // private void OnFingerDown(Finger finger)
    // {
    //     //finger.screenPosition
    // }
    //
    // public static Vector2 GetMousePosition()
    // {
    //     return Mouse.current.position.ReadValue();
    // }
}