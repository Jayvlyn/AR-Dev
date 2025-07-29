using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class LockOnTarget : MonoBehaviour
{
    
    private IEnumerator RestoreRotation(float time)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.identity;

        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / time);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, normalizedTime);
            yield return null;
        }
        transform.rotation = targetRotation;
        //ARPlaneManager obj;
        //obj.
    }
    
    private bool locked;
    [SerializeField] private UFOController ufoController;
    public void Lock()
    {
        locked = true;
        ufoController.StopAttacking();
    }

    [SerializeField] private float timeToRestoreRotationAfterThrow = 1f;
    public void Unlock()
    {
        locked = false;
        ufoController.AllowAttacking();
        StartCoroutine(RestoreRotation(timeToRestoreRotationAfterThrow));
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