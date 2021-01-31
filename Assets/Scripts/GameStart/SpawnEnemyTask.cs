using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Medic/Tasks/Spawn Enemy")]
public class SpawnEnemyTask : StartGameTask
{
	[SerializeField]
	private Set_AdventurerSpanwPoints spawnPoints = default;

	[SerializeField]
	private KIController[] enemiesToSpawn = default;

	public override void Execute(StartGameScript startGameScript, Action onDone)
	{
		List<int> indices = new List<int>(spawnPoints.Count);
		for (int i = 0; i < spawnPoints.Count; ++i)
			indices.Add(i);

		if (indices.Count < enemiesToSpawn.Length)
		{
			Debug.LogErrorFormat("Not enough enemy spawn points found. Needed {0}, given {1}", enemiesToSpawn.Length, spawnPoints.Count);
			onDone();
			return;
		}

		for (int i = 0; i < enemiesToSpawn.Length; ++i)
		{
			int index = UnityEngine.Random.Range(0, indices.Count);
			var spawn = spawnPoints[indices[index]];
			var enemy = Instantiate(enemiesToSpawn[i], spawn.transform.position, spawn.transform.rotation);

            enemy.GetComponent<KIController>().AddWaypoints(spawn.GetComponent<WaypointList>().Waypoints);
			//Debug.LogError("We've not given the enemy a patrol route!!!");

			indices.RemoveAt(index);
		}

		onDone();
	}
}
