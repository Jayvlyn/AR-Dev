using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WaveController : MonoBehaviour
{
    public static WaveController instance;

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

    private void Awake()
    {
        instance = this;
    }

    public void OnStart()
    {
        TimeSurvived = 0;
        currentEnemyPoints = enemyValues.maxPoints;
        InitializeValues();
        nextEnemyPointValueToSpawn = SetNextSpawnValue(enemyValues.spawnableList);
    }

    private void Update()
    {
        if(GameManager.instance.gameActive)
        {
            TimeSurvived += Time.deltaTime;

            CheckEnemyTimers();
            CheckWaveTimers();
        }
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
        enemyStrength++;

        if (enemyWaveValues[currentWave] == null)
        {
            Debug.LogWarning($"There isn't a wave {currentWave}");
            return;
        }

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