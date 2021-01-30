using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{ 
    [SerializeField]
    private ParticleSystem[] _particlesToHandle;

    private void Awake()
    {
        _particlesToHandle = GetComponentsInChildren<ParticleSystem>();
    }

    public void PlayAllParicle()
    {
        foreach (ParticleSystem ps in _particlesToHandle)
        {
            ps.Play();
        }
    }
    public void PlayAllParicle(Vector3 poistion)
    {
        foreach (ParticleSystem ps in _particlesToHandle)
        {
            ps.transform.position = poistion;
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

    public void ToggleAllParticles(bool toggle, Vector3 position)
    {
        if (toggle)
        {
            PlayAllParicle(position);
        }
        else
        {
            StopAllParticles();
        }
    }


}
