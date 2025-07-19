using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LifeVisual : MonoBehaviour
{
    public static Action OnLifeRemoved;
    
    [Button("Kill")]
    public void Remove()
    {
        OnLifeRemoved?.Invoke();
    }
}