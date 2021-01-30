using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondtionTester : MonoBehaviour
{
    [SerializeField]
    private Condition _CondtionToTest;

    private void OnEnable()
    {
        _CondtionToTest.OnConditionRaised += OnRaise;
    }

    private void OnDisable()
    {
        _CondtionToTest.OnConditionRaised -= OnRaise;
    }

    private void OnRaise()
    {
        Debug.Log("I got called!");
    }

}
