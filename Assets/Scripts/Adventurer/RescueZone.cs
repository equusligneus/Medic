using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class RescueZone : MonoBehaviour
{
    [SerializeField]
    private ParticleController _DespawnParticle;

    AudioSource audioData;

    private void Awake()
    {
        audioData = GetComponent<AudioSource>();
    }



    private void OnTriggerEnter(Collider other)
    {
        
        Adventurer adv = other.GetComponentInParent<Adventurer>();
        if(adv != null)
        {
            _DespawnParticle.ToggleAllParticles(true, adv.transform.position);
            audioData.Play(0);
            adv.GotRescued();
        }
    }
}
