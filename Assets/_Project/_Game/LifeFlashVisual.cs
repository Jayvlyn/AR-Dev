using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LifeFlashVisual : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private float flashDuration;
    [SerializeField] private float maxIntensity;

    private void OnEnable()
    {
        LifeTracker.OnLifeCountChanged += Flash;
    }

    private void OnDisable()
    {
        LifeTracker.OnLifeCountChanged -= Flash;
    }

    private bool flashing;
    private float timeOfFlashStart;
    public void Flash(int count)
    {
        if (count == 10) return;
        flashing = true;
        timeOfFlashStart = Time.time;
    }

    private void Update()
    {
        if (flashing)
        {
            float elapsed = Time.time - timeOfFlashStart;

            // This makes intensity go from 0 → 1 → 0 over flashDuration seconds
            float pingPong = Mathf.PingPong(elapsed * 2f, flashDuration);
            vignette.intensity.value = (pingPong / flashDuration) * maxIntensity;

            if (elapsed >= flashDuration)
                flashing = false;
        }
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    private Vignette vignette;

    void Start()
    {
        if (volume.profile.TryGet(out vignette))
        {
            
        }
        else
        {
            Debug.LogWarning("No Vignette override found in volume profile.");
        }
    }
}
