using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WaveController : MonoBehaviour
{
    public static float TimeSurvived;

    [Header("General things")]
    [SerializeField] private float waveLength;
    private float waveTimer;
    private int currentWave;

    [Header("Enemy things")]
    [SerializeField, Tooltip("Wave 1 NEEDS to have a strength to enter of 1")] private List<WaveValueSpecifier> enemyWaveValues = new();
    private WaveValues enemyValues;
    private int enemyStrength;
    private int currentEnemyPoints;

    private float enemyPointRecoverTimer;
    private float enemySpawnTimer;

    private int nextEnemyPointValueToSpawn;

    [Header("Item things")]
    [SerializeField, Tooltip("Wave 1 NEEDS to have a strength to enter of 1")] private List<WaveValueSpecifier> itemWaveValues = new();
    private WaveValues itemValues;
    private int itemStrength;
    private int currentItemPoints;

    private float itemPointRecoverTimer;
    private float itemSpawnTimer;

    private int nextItemPointValueToSpawn;

    private void Start()
    {
        currentEnemyPoints = enemyValues.maxPoints;
        InitializeValues();
        nextEnemyPointValueToSpawn = SetNextSpawnValue(enemyValues.spawnableList);
        nextItemPointValueToSpawn = SetNextSpawnValue(itemValues.spawnableList);
    }

    private void Update()
    {
        TimeSurvived += Time.deltaTime;

        CheckSpawnTimer(ref enemySpawnTimer, enemyValues, ref currentEnemyPoints, ref nextEnemyPointValueToSpawn);
        CheckSpawnTimer(ref itemSpawnTimer, itemValues, ref currentItemPoints, ref nextItemPointValueToSpawn);
        CheckPointRecoveryTimer(ref enemyPointRecoverTimer, enemyValues, ref currentEnemyPoints);
        CheckPointRecoveryTimer(ref itemPointRecoverTimer, itemValues, ref currentItemPoints);
        CheckWaveTimers();
    }

    private void CheckSpawnTimer(ref float timerToCheck, WaveValues values, ref int currentPoints, ref int nextToSpawn)
    {
        timerToCheck -= Time.deltaTime;
        


        if (timerToCheck <= 0 && currentPoints >= nextToSpawn)
        {
            SpawnNewItem(nextToSpawn, values.spawnableList, ref currentPoints);
            timerToCheck = values.timeBetweenSpawns;
            nextToSpawn = SetNextSpawnValue(values.spawnableList);
        }

        enemyPointRecoverTimer -= Time.deltaTime;

        if (enemyPointRecoverTimer <= 0)
        {
            currentEnemyPoints += enemyValues.pointRecoverAmount;
            currentEnemyPoints = Mathf.Clamp(currentEnemyPoints, 0, enemyValues.maxPoints);
            enemyPointRecoverTimer = enemyValues.pointRecoverLength;
        }
    }

    private void CheckPointRecoveryTimer(ref float timerToCheck, WaveValues values, ref int currentPoints)
    {
        timerToCheck -= Time.deltaTime;

        if (timerToCheck <= 0)
        {
            currentPoints += values.pointRecoverAmount;
            currentPoints = Mathf.Clamp(currentPoints, 0, values.maxPoints);
            timerToCheck = values.pointRecoverLength;
        }
    }

    private void CheckWaveTimers()
    {
        waveTimer -= Time.deltaTime;

        if (waveTimer <= 0)
        {
            waveTimer = waveLength;
            NextWave();
        }
    }

    private void SpawnNewItem(int value, List<Spawnable> spawnables, ref int currentPoints)
    {
        List<GameObject> spawnableThings = new();
        foreach(Spawnable spawnable in spawnables)
        {
            if (spawnable.costToSpawn == value)
            {
                spawnableThings.Add(spawnable.objectToSpawn);
            }
        }

        if (spawnableThings.Count > 0)
        {
            GameObject spawnable = spawnableThings[UnityEngine.Random.Range(0, spawnableThings.Count)];
            currentPoints -= value;
            Instantiate(spawnable, GetSpawnPointInBoundsRandomly(), transform.rotation, transform);
            return;
        }

        Debug.LogWarning($"Couldn't find a spawnable of value {value}");
    }

    [SerializeField] private Bounds spawnArea;

    private Vector3 GetSpawnPointInBoundsRandomly()
    {
        return transform.position + new Vector3(
            spawnArea.min.x + UnityEngine.Random.Range(-spawnArea.size.x, spawnArea.size.x),
            spawnArea.min.y + UnityEngine.Random.Range(-spawnArea.size.y, spawnArea.size.y),
            spawnArea.min.z + UnityEngine.Random.Range(-spawnArea.size.z, spawnArea.size.z)
        );

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + spawnArea.center, spawnArea.size);
    }

    private void InitializeValues()
    {
        NextWave();
    }

    private int SetNextSpawnValue(List<Spawnable> spawnables)
    {
        return UnityEngine.Random.Range(1, spawnables[spawnables.Count - 1].costToSpawn + 1);
    }

    private void NextWave()
    {
        #region Enemy Wave Check
        enemyStrength++;
        itemStrength++;

        if (enemyWaveValues[currentWave] == null)
        {
            Debug.LogWarning($"There isn't an enemy wave for {currentWave}");
            return;
        }
        else if (enemyStrength == enemyWaveValues[currentWave].strengthToEnterWave)
        {
            enemyValues = enemyWaveValues[currentWave].GetWaveValues();
            currentEnemyPoints = enemyValues.maxPoints;
            enemySpawnTimer = 0;
            enemyPointRecoverTimer = enemyValues.pointRecoverLength;
            currentWave++;
        }

        if (itemWaveValues[currentWave] == null)
        {
            Debug.LogWarning($"There isn't an item wave for {currentWave}");
            return;
        }
        else if (itemStrength == itemWaveValues[currentWave].strengthToEnterWave)
        {
            itemValues = enemyWaveValues[currentWave].GetWaveValues();
            currentItemPoints = itemValues.maxPoints - 1;
            itemSpawnTimer = 0;
            itemPointRecoverTimer = itemValues.pointRecoverLength;
        }
        #endregion
    }
}

#region Classes
[Serializable]
public class Spawnable
{
    public GameObject objectToSpawn;
    public int costToSpawn;
}

[Serializable]
public class WaveValueSpecifier
{
    [Tooltip("For items, make this value the same as the enemies")]public int strengthToEnterWave;
    [SerializeField] private WaveValues WaveValues;

    public WaveValues GetWaveValues()
    {
        return WaveValues;
    }
}

[Serializable]
public struct WaveValues
{
    [Tooltip("PLEASE put the highest value spawnable as the last value")]public List<Spawnable> spawnableList;
    public int maxPoints;
    public int pointRecoverAmount;
    public float pointRecoverLength;
    public float timeBetweenSpawns;
}

#endregion