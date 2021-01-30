using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame_Controller : MonoBehaviour
{
    [SerializeField]
    private Set_Adventurer _SetAdventurers;

    [SerializeField]
    private Ref_Bool _isPlayerAlive;

    public event Action LostGame;

    [SerializeField]
    private Condition _winCondition;

    [SerializeField]
    private Condition _loseCondition;


    private void OnEnable()
    {
        _isPlayerAlive.OnChanged += OnPlayerDied;
        _SetAdventurers.OnCountChanged += OnAdnventurerRescuded;

    }

    private void OnDisable()
    {
        _isPlayerAlive.OnChanged -= OnPlayerDied;
        _SetAdventurers.OnCountChanged -= OnAdnventurerRescuded;
    }


    /// <summary>
    /// Checks if the player died and calls the lose condition
    /// </summary>
    private void OnPlayerDied()
    {
        if (!_isPlayerAlive.Get())
        {
            Debug.Log("Player died in EndGame");
            _loseCondition.RaiseCondition();
        }
    }


    /// <summary>
    /// Checks if all the Advenurers got rescued and if so calls the win condition
    /// </summary>
    private void OnAdnventurerRescuded()
    {
        for (int i = 0; i < _SetAdventurers.Count; i++)
        {
            Adventurer adv = _SetAdventurers[i];

            if (!adv.IsRescued)
            {
                return;
            }
        }

        _winCondition.RaiseCondition();
    }
}
