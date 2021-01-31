using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoorSteps : MonoBehaviour
{
    [SerializeField]
    public ParticleController _FootSetpParticle;


    public void FootStepLeft()
    {
        _FootSetpParticle.PlayAllParicle();
    }

    public void FootStepRight()
    {
        _FootSetpParticle.PlayAllParicle();
    }
}
