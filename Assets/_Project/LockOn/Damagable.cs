using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private UnityEvent OnDeath;
    public void TakeDamage(int damage = 1)
    {
        health -= damage;
        if(health <= 0) OnDeath.Invoke();
    }
}