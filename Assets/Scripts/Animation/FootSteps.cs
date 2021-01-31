using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootSteps : MonoBehaviour
{
    [SerializeField]
    private ParticleController _FootSetpParticle;

    [SerializeField]
    public UnityEvent _LeftStepEvent;

    [SerializeField]
    private UnityEvent _RightStepEvent;

    public void FootStepLeft()
    {
        _FootSetpParticle?.PlayAllParicle();
        _LeftStepEvent?.Invoke();
    }

    public void FootStepRight()
    {
        _FootSetpParticle?.PlayAllParicle();
        _RightStepEvent?.Invoke();
    }
}
