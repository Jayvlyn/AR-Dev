using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField] private GameObject audioObjectPrefab;
    [SerializeField] private List<AudioClip> mooList = new();

    [SerializeField] private List<AudioClip> explosions = new();

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


    public void PlayMooOneShot(Vector2 pitchMinMax, Vector3 position)
    {
        PlayOneShotAtLocation(mooList[Random.Range(0, mooList.Count)], pitchMinMax, position);
    }

    public void PlayExplosionOneShot(Vector2 pitchMinMax, Vector3 position)
    {
        PlayOneShotAtLocation(explosions[Random.Range(0, mooList.Count)], pitchMinMax, position);
    }

}
