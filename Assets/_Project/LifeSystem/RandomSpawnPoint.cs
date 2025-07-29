using UnityEngine;

public class RandomSpawnPoint : LifeAreaSpawnMethod
{
    public override Vector3 GetSpawnPoint(int planeIndex)
    {
        WanderableArea area = WanderableArea.activeWanderableArea;

        return area.GetRandomPosition(planeIndex);
    }
}


public abstract class LifeAreaSpawnMethod : MonoBehaviour
{
    public abstract Vector3 GetSpawnPoint(int planeIndex);
}
