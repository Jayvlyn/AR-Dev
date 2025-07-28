using UnityEngine;

public class LifeSpawner : MonoBehaviour
{
    [SerializeField] private LifeVisual visualPrefab;
    [SerializeField] private LifeAreaSpawnMethod spawnMethod;
    public void Spawn()
    {
        var visual = Instantiate(visualPrefab, spawnMethod.GetSpawnPoint(), Quaternion.identity, transform);
    }
}