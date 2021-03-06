﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack_Ability : MonoBehaviour
{
    [SerializeField]
    private Ref<Transform> _playerTransformRef;

    [SerializeField]
    private float _AttackRange;

    [SerializeField]
    private float _AttackCooldDown;

    [SerializeField]
    private int _AttackDamage = 1;

    public float _lastHit = float.MinValue;

    /// <summary>
    /// Checks if the Player is in Range
    /// </summary>
    /// <param name="player">The transform of the Player</param>
    /// <returns>True if the player is in range</returns>
    public bool IsPlayerInRange(Transform player)
    {
        if(Time.time < _lastHit + _AttackCooldDown)
        {
            return false;
        }

        float dist = Vector3.Distance(this.transform.position, player.position);

        return dist <= _AttackRange;
    }


    public void AttackPlayer()
    {
        AttackPlayer(_playerTransformRef.Get());
    }

    /// <summary>
    /// Attacks the given player throw a Log if the given player has no health
    /// </summary>
    /// <param name="player"></param>
    public void AttackPlayer(Transform player)
    {
        MedicHealth mh = player.GetComponentInParent<MedicHealth>();

        if(mh == null)
        {
            Debug.Log("No PlayerHealth found");
            return;
        }

        mh.Hit(_AttackDamage);
        _lastHit = Time.time;
    }



}
