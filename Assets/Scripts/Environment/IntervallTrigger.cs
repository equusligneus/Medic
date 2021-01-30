using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IntervallTrigger : TriggerPlate
{
    [SerializeField]
    private bool _run = true;

    private bool _active;

    [Tooltip("This ins important, if used with triggers that needs to be untrigger as well")]
    [SerializeField]
    private bool _triggerAndUntrigger;
    private bool _trigger = true;

    [SerializeField]
    private float _intervallTime;


    private float _lastIntervall = float.MinValue;


    private void Update()
    {
        HandleIntervall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_run)
        {
            return;
        }

        if (other.GetComponent<MedicHealth>() != null)
        {
            _active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_run)
        {
            return;
        }

        if (other.GetComponent<MedicHealth>() != null)
        {
            _active = false;
        }

    }

    public void ToggleActive(bool toggle)
    {
        _run = toggle;
        _active = toggle;
    }

    private void HandleIntervall()
    {
        if (!_active || Time.time < _lastIntervall + _intervallTime)
        {
            return;
        }


        if (!_triggerAndUntrigger)
        {
            TriggerAllTriggers();
        }
        else
        {
            if (_trigger)
            {
                TriggerAllTriggers();
            }
            else
            {
                UnTriggerAllTriggers();
            }

            _trigger = !_trigger;
        }

        _lastIntervall = Time.time;
    }
}
