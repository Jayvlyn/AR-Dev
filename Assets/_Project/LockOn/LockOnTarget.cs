using System;
using UnityEngine;
using UnityEngine.Events;

public class LockOnTarget : MonoBehaviour
{
    private bool locked;
    [SerializeField] private UFOController ufoController;
    public void Lock()
    {
        locked = true;
        ufoController.StopAttacking();
    }

    public void Unlock()
    {
        locked = false;
        ufoController.AllowAttacking();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (locked)
        {
            if (other.gameObject.TryGetComponent(out LockOnTarget target))
            {
                target.Smack();
            }
            Smack();
        }

    }

    [SerializeField] private UnityEvent OnSmacked;
    public void Smack()
    {
        OnSmacked.Invoke();
    }
}