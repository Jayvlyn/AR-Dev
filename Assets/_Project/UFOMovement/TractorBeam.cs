using System;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject tractorBeam;

    private void Awake()
    {
        particles.Stop();
    }

    public void Reveal()
    {
        particles.Play();
        tractorBeam.SetActive(true);
    }

    public void Hide()
    {
        particles.Stop();
        tractorBeam.SetActive(false);
    }
}