using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Triggerable_DamageArea : ATriggerable
{
    [SerializeField]
    private ParticleController _particleShow;

    private Collider _DamageArea;

    [SerializeField]
    private int _damage = 1;

    [SerializeField]
    private bool _startOn;

    [SerializeField]
    private float _DelayTurnOnTime;

    private void Awake()
    {
        _DamageArea = GetComponent<Collider>();
        _DamageArea.enabled = false;
        _particleShow.ToggleAllParticles(_startOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        MedicHealth mh = other.GetComponent<MedicHealth>();
        if (mh != null)
        {
            mh.Hit(_damage);
        }
    }

    public override void GotTriggered(TriggerPlate by)
    {
        if (!_startOn)
        {
            StartCoroutine(DelayGotTriggered());
            return;
        }

        _particleShow.ToggleAllParticles(!_startOn);
        _DamageArea.enabled = !_startOn;
    }

    private IEnumerator DelayGotTriggered()
    {
        yield return new WaitForSeconds(_DelayTurnOnTime);
        _particleShow.ToggleAllParticles(!_startOn);
        _DamageArea.enabled = !_startOn;
    }

    public override void GotUnTriggered(TriggerPlate by)
    {
        if (_startOn)
        {
            StartCoroutine(DelayGotUnTriggered());
            return;
        }

        _particleShow.ToggleAllParticles(_startOn);
        _DamageArea.enabled = _startOn;
    }

    private IEnumerator DelayGotUnTriggered()
    {
        yield return new WaitForSeconds(_DelayTurnOnTime);
        _particleShow.ToggleAllParticles(_startOn);
        _DamageArea.enabled = _startOn;
    }

}
