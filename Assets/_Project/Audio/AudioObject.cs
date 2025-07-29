using UnityEngine;

public class AudioObject : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private float lifespan;

    public void SetValues(AudioClip clip, Vector2 pitchMinMax)
    {
        audioSource.pitch = Random.Range(pitchMinMax.x, pitchMinMax.y);

        audioSource.PlayOneShot(clip);

        Destroy(gameObject, clip.length);
    }
}
