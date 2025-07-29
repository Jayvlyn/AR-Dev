using UnityEngine;

public class LifeSpawner : MonoBehaviour
{
    [SerializeField] private LifeVisual visualPrefab;
    [SerializeField] private LifeAreaSpawnMethod spawnMethod;
    public void Spawn()
    {
        WanderableArea area = WanderableArea.activeWanderableArea;
        int planeIndex = Random.Range(0, area.colliders.Count);

        var visual = Instantiate(visualPrefab, spawnMethod.GetSpawnPoint(planeIndex), Quaternion.identity, transform);
        WanderingMovement cow = visual.GetComponent<WanderingMovement>();
        cow.planeIndex = planeIndex;
    }
}