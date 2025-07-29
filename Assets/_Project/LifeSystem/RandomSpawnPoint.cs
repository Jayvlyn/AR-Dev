using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnPoint : LifeAreaSpawnMethod
{
    public override Vector3 GetSpawnPoint(int planeIndex)
    {
        WanderableArea area = WanderableArea.activeWanderableArea;
        Collider plane = area.colliders[planeIndex];

        return area.GetRandomPosition(planeIndex);
    }
}


public abstract class LifeAreaSpawnMethod : MonoBehaviour
{
    public abstract Vector3 GetSpawnPoint(int planeIndex);
}
