using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set_AdventurerSpawnPoint", menuName = "Medic/Set/AdventurerSpawnPoint")]
public class Set_AdventurerSpanwPoints : RuntimeSet<AdventurerSpawnPoint>
{
    [SerializeField]
    private Set_Adventurer _AdventurerSet;

    /// <summary>
    /// Retruns an array with the transforms of the right amount of spawnpoints according to the amount of adventurers in swzene
    /// </summary>
    /// <returns></returns>
    public Transform[] GetRandomSpawnPoints()
    {
        List<Transform> spawnpoint = new List<Transform>();

        for (int i = 0; i < _AdventurerSet.Count; i++)
        {
            Transform ret;
            do
            {
                int rnd = Random.Range(0, this.Count);
                ret = this[rnd].transform;
            } while (spawnpoint.Contains(ret));

            spawnpoint.Add(ret);
        }

        return spawnpoint.ToArray();
    }
}
