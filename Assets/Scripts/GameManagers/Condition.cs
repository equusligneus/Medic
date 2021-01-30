using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default_GameCondition", menuName = "Medic/GameConditions")]
public class Condition : ScriptableObject
{
    /// <summary>
    /// The Event that gets called, 
    /// </summary>
    public event Action OnConditionRaised = default;

    /// <summary>
    /// Call this, if you want to call the event
    /// </summary>
    public void RaiseCondition()
    {
        OnConditionRaised?.Invoke();
    }
}
