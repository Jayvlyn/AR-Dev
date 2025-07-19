using System;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private int initialCount;
    private LifeTracker tracker;
    [SerializeField] private LifeSpawner spawner;

    private void Awake()
    {
        tracker = new LifeTracker(initialCount);
        for (int i = 0; i < initialCount; i++)
        {
            spawner.Spawn();
        }
    }

    private void OnEnable()
    {
        LifeVisual.OnLifeRemoved += OnLifeLost;
    }

    private void OnDisable()
    {
        LifeVisual.OnLifeRemoved -= OnLifeLost;
    }

    private void OnLifeLost()
    {
        tracker.Remove();
    }
}

public class LifeTracker
{
    private int initialLifeCount;
    public static Action<int> OnLifeCountChanged;

    private int lifeCount;
    private int LifeCount {
        get { return lifeCount; }
        set { 
            lifeCount = value;
            OnLifeCountChanged?.Invoke(value);
        }
    }

    public LifeTracker(int startingLifeCount)
    {
        initialLifeCount = startingLifeCount;
        LifeCount = startingLifeCount;
    }
    
    

    public void Add(int amount = 1)
    {
        if (amount == 0) return;
        LifeCount += amount;
    }
    public void Remove(int amount = 1)
    {
        if (amount == 0) return;
        LifeCount -= amount;
    }
    public float GetHealthPercentage() => (float)LifeCount / initialLifeCount;
}