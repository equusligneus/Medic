using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Medic/Tasks/DunGen")]
public class GenerateDungeonTask : StartGameTask
{
	[SerializeField]
	private DungeonMapGenerator generator;

	public override void Execute(StartGameScript startGameScript, Action onDone)
	{
		startGameScript.StartCoroutine(GenerateDungeon(startGameScript, onDone));
	}

	IEnumerator GenerateDungeon(StartGameScript startGameScript, Action onDone)
	{
		var generator = Instantiate(this.generator);
		generator.SetStartRoom(startGameScript.GetComponent<DungeonRoom>());
		generator.GenerateDungeon();
		yield return null;
		onDone();
	}
}
