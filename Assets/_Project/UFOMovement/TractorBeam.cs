using System;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particles = new();

    private void Awake()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }

    public void Reveal()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    public void Hide()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }
}