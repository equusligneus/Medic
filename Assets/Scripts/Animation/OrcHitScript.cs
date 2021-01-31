using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrcHitScript : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _HitEvent;

    public void OrcHit()
    {
        _HitEvent?.Invoke();
    }
}
