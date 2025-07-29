using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public bool gameActive = false;

    public void StartGame()
    {
        LifeManager.instance.SpawnCows();
        WaveController.instance.OnStart();
    }
}
