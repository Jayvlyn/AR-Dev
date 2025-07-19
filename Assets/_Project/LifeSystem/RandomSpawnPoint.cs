using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnPoint : LifeAreaSpawnMethod
{
    [SerializeField] private List<Transform> spawnPoints;
    public override Vector3 GetSpawnPoint() => spawnPoints[Random.Range(0, spawnPoints.Count)].position;
}


public abstract class LifeAreaSpawnMethod : MonoBehaviour
{
    public abstract Vector3 GetSpawnPoint();
}
