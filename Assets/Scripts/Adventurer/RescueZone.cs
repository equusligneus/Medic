using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RescueZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Adventurer adv = other.GetComponentInParent<Adventurer>();
        if(adv != null)
        {
            adv.GotRescued();
        }
    }
}
