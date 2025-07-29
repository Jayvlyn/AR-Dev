using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField] private GameObject audioObjectPrefab;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public void PlayOneShotAtLocation(AudioClip sound, Vector2 pitchMinMax, Vector3 position)
    {
        GameObject spawned = Instantiate(audioObjectPrefab, position, transform.rotation);
        AudioObject spawnedAO = spawned.GetComponent<AudioObject>();
        spawnedAO.SetValues(sound, pitchMinMax);
    }
}
