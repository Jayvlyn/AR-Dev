using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField] private GameObject audioObjectPrefab;
    [SerializeField] private List<AudioClip> mooList = new();
    [SerializeField] private AudioClip eatingSound;

    [SerializeField] private List<AudioClip> explosions = new();
    [SerializeField] private AudioClip waveStart;

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

    public void PlayEatingOneShot(Vector2 pitchMinMax, Vector3 position)
    {
        PlayOneShotAtLocation(eatingSound, pitchMinMax, position);

    }

    public void PlayExplosionOneShot(Vector2 pitchMinMax, Vector3 position)
    {
        PlayOneShotAtLocation(explosions[Random.Range(0, mooList.Count)], pitchMinMax, position);
    }

    public void PlayWaveStartOneShot()
    {
        PlayOneShotAtLocation(waveStart, new Vector2(1, 1), Vector3.zero);
    }

}
