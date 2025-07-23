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
    private WaveValues enemyValues;
    [SerializeField, Tooltip("Wave 1 NEEDS to have a strength to enter of 1")] private List<WaveValueSpecifier> enemyWaveValues = new();
    private int enemyStrength;
    private int currentEnemyPoints;

    private float pointRecoverTimer;
    private float enemySpawnTimer;

    private int nextEnemyPointValueToSpawn;

    private void Start()
    {
        currentEnemyPoints = enemyValues.maxPoints;
        InitializeValues();
        nextEnemyPointValueToSpawn = SetNextSpawnValue(enemyValues.spawnableList);
    }

    private void Update()
    {
        TimeSurvived += Time.deltaTime;

        CheckEnemyTimers();
        CheckWaveTimers();
    }

    private void CheckEnemyTimers()
    {
        enemySpawnTimer -= Time.deltaTime;
        


        if (enemySpawnTimer <= 0 && currentEnemyPoints >= nextEnemyPointValueToSpawn)
        {
            SpawnNewItem(nextEnemyPointValueToSpawn, enemyValues.spawnableList, ref currentEnemyPoints);
            enemySpawnTimer = enemyValues.timeBetweenSpawns;
            Debug.Log("Spawned Item");
            nextEnemyPointValueToSpawn = SetNextSpawnValue(enemyValues.spawnableList);
        }

        pointRecoverTimer -= Time.deltaTime;

        if (pointRecoverTimer <= 0)
        {
            currentEnemyPoints += enemyValues.pointRecoverAmount;
            currentEnemyPoints = Mathf.Clamp(currentEnemyPoints, 0, enemyValues.maxPoints);
            pointRecoverTimer = enemyValues.pointRecoverLength;
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
            Instantiate(spawnable, new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10)), transform.rotation);
            return;
        }

        Debug.LogWarning($"Couldn't find a spawnable of value {value}");
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
        enemyStrength++;
        if (enemyStrength == enemyWaveValues[currentWave].strengthToEnterWave)
        {
            enemyValues = enemyWaveValues[currentWave].GetWaveValues();
            currentEnemyPoints = enemyValues.maxPoints;
            enemySpawnTimer = 0;
            pointRecoverTimer = enemyValues.pointRecoverLength;
            currentWave++;
        }
    }
}

[Serializable]
public class Spawnable
{
    public GameObject objectToSpawn;
    public int costToSpawn;
}

[Serializable]
public class WaveValueSpecifier
{
    public int strengthToEnterWave;
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