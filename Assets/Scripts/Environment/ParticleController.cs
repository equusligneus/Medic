using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _particlesToHandle;

    public void PlayAllParicle()
    {
        foreach (ParticleSystem ps in _particlesToHandle)
        {
            ps.Play();
        }
    }

    public void StopAllParticles()
    {
        foreach (ParticleSystem ps in _particlesToHandle)
        {
            ps.Stop();
        }
    }

    public void ToggleAllParticles(bool toggle)
    {
        if (toggle)
        {
            PlayAllParicle();
        }
        else
        {
            StopAllParticles();
        }
    }
}
