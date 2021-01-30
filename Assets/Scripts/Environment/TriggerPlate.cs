using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerPlate : MonoBehaviour
{
    [SerializeField]
    protected List<ATriggerable> _Triggerables = new List<ATriggerable>();

    [SerializeField]
    protected bool _callUnTriggerOnTargets = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerAllTriggers();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnTriggerAllTriggers();
        }
    }

    protected virtual void TriggerAllTriggers()
    {
        foreach (ATriggerable tigger in _Triggerables)
        {
            tigger.GotTriggered(this);
        }
    }

    protected virtual void UnTriggerAllTriggers()
    {
        if (!_callUnTriggerOnTargets)
        {
            return;
        }

        foreach(ATriggerable trigger in _Triggerables)
        {
            trigger.GotUnTriggered(this);
        }
    }

    /// <summary>
    /// Removes the given Triggerable from the List of the triggables
    /// </summary>
    /// <param name="toRemove">The Triggerable to remove</param>
    public virtual void RemoveTriggerableFromList(ATriggerable toRemove)
    {
        _Triggerables.Remove(toRemove);
    }

}


