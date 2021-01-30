using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RescueZone : MonoBehaviour
{
    [SerializeField]
    private ParticleController _DespawnParticle;
    private void OnTriggerEnter(Collider other)
    {
        Adventurer adv = other.GetComponentInParent<Adventurer>();
        if(adv != null)
        {
            _DespawnParticle.ToggleAllParticles(true, adv.transform.position);
            adv.GotRescued();
        }
    }
}
