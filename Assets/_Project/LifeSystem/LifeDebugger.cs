using System;
using TMPro;
using UnityEngine;

public class LifeDebugger : MonoBehaviour
{
    [SerializeField] private TMP_Text textAsset;

    private void OnEnable()
    {
        LifeTracker.OnLifeCountChanged += SetLifeCount;
    }

    private void OnDisable()
    {
        LifeTracker.OnLifeCountChanged -= SetLifeCount;
    }

    public void SetLifeCount(int lifeCount) => textAsset.text = "Lives: " + lifeCount.ToString();
}
