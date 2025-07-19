using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Abducatable : MonoBehaviour
{
    [SerializeField] private UnityEvent OnBecameFullyAbducted;

    public void StartAbduction(Abductor abductor)
    {
        
    }

    [Button("Abduct")]
    public void CompleteAbduction()
    {
        OnBecameFullyAbducted?.Invoke();
    }
    public void FailAbduction()
    {
        
    }
}

public class Abductor : MonoBehaviour
{
    
}
