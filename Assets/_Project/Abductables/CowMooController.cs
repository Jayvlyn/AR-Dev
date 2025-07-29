using UnityEngine;

public class CowMooController : MonoBehaviour
{
    [SerializeField] private float timeBetweenMoos;
    [SerializeField, Tooltip("Treat this as a min max slider")] private Vector2 mooTimeRandomChange;
    [SerializeField] private Vector2 mooPitchMinMax;

    private float mooTimer;

    private void Awake()
    {
        ResetMooTimer();
    }

    private void Update()
    {
        CheckMooTimer();
    }

    private void CheckMooTimer()
    {
        mooTimer -= Time.deltaTime;

        if (mooTimer <= 0)
        {
            AudioController.instance.PlayMooOneShot(mooPitchMinMax, transform.position);
            ResetMooTimer();
        }
    }

    private void ResetMooTimer()
    {
        mooTimer = timeBetweenMoos + (Random.Range(mooTimeRandomChange.x, mooTimeRandomChange.y));
    }
}
