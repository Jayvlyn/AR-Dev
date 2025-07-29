using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI text;

    [SerializeField] private ARPlaneManager planeManager;

    [HideInInspector] public bool gameActive = false;
    [HideInInspector] public bool scanning = false;

    private void Awake()
    {
        instance = this;
    }

    public void StartScan()
    {
        EnablePlaneManager();
        scanning = true;
    }

    public void StartGame()
    {
        DisablePlaneManager();
        scanning = false;
        LifeManager.instance.SpawnCows();
        WaveController.instance.OnStart();
        gameActive = true;
    }

    public void OnGameEnd()
    {
        UIManager.instance.ShowEndPanel();
        gameActive = false;
    }

    public void EnablePlaneManager()
    {
        //planeManager.enabled = true;
    }

    public void DisablePlaneManager()
    {
        //planeManager.enabled = false;
    }
}
