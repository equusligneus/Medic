using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Set_AdventurerSpanwPoints _SpawnPoints = default;

    private void OnEnable()
    {
        _SpawnPoints.Add(this);
    }

    private void OnDisable()
    {
        _SpawnPoints.Remove(this);
    }

}
