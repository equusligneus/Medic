using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Medic/Tasks/NavGen")]
public class GenerateNavMeshTask : StartGameTask
{
	[SerializeField]
	private GridManager gridManager = default;

	public override void Execute(StartGameScript startGameScript, Action onDone)
	{
		Instantiate(gridManager).GenerateGrid();
		onDone();
	}
}
