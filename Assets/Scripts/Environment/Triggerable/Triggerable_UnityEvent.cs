using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Triggerable_UnityEvent : ATriggerable
{
    [SerializeField]
    private UnityEvent _myEvent;

    public override void GotTriggered(TriggerPlate by)
    {
        _myEvent.Invoke();
    }

}
