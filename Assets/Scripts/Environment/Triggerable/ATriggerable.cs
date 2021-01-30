using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ATriggerable : MonoBehaviour
{
    public virtual void GotTriggered(TriggerPlate by)
    {
        
    }

    public virtual void GotUnTriggered(TriggerPlate by)
    {

    }
}

