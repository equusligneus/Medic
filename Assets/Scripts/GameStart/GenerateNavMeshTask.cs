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
        startGameScript.StartCoroutine(GenerateGrid(onDone));
	}

    IEnumerator GenerateGrid(Action onDone)
    {

        GridManager temp = Instantiate(gridManager);
        Debug.Log("Position: " + DungeonMapGenerator.MapSize.GetCenter + " Size: " + DungeonMapGenerator.MapSize.GetWidth() + " / " + DungeonMapGenerator.MapSize.GetHeight());
        temp.transform.position = DungeonMapGenerator.MapSize.GetCenter;
        temp.Size = new Vector2Int(DungeonMapGenerator.MapSize.GetWidth(), DungeonMapGenerator.MapSize.GetHeight());
        temp.GenerateGrid();

        yield return null;
        onDone();
    }
}
