using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Medic/Tasks/Spawn Adventurer")]
public class SpawnAdventurerTask : StartGameTask
{
	[SerializeField]
	private Set_AdventurerSpanwPoints spawnPoints = default;

	[SerializeField]
	private Adventurer[] adventurersToSpawn = default;

	public override void Execute(StartGameScript startGameScript, Action onDone)
	{
		List<int> indices = new List<int>(spawnPoints.Count);
		for (int i = 0; i < spawnPoints.Count; ++i)
			indices.Add(i);

		if (indices.Count < adventurersToSpawn.Length)
		{
			Debug.LogErrorFormat("Not enough adventurer spawn points found. Needed {0}, given {1}", adventurersToSpawn.Length, spawnPoints.Count);
			onDone();
			return;
		}

		for (int i = 0; i < adventurersToSpawn.Length; ++i)
		{
			int index = UnityEngine.Random.Range(0, indices.Count);
			var spawn = spawnPoints[indices[index]];
			Instantiate(adventurersToSpawn[i], spawn.transform.position, spawn.transform.rotation);
			indices.RemoveAt(index);
		}

		onDone();
	}


}
