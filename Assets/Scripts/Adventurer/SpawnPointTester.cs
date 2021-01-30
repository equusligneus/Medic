using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTester : MonoBehaviour
{
    [SerializeField]
    private Set_AdventurerSpanwPoints _spawnPoints = default;

    [ContextMenu("Give Spawnpoints")]
    private void GetSpanwPoints()
    {
        Transform[] spanwpoints = _spawnPoints.GetRandomSpawnPoints();

        foreach(Transform trans in spanwpoints)
        {
            Debug.Log($"Point: {trans.name}");
        }

    }
}
