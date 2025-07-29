using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    [SerializeField] private float destructionLength;

    private void Start()
    {
        Destroy(gameObject, destructionLength);
    }
}
