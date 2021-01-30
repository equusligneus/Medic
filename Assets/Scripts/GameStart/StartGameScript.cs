using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StartGameTask : ScriptableObject
{
	public abstract void Execute(StartGameScript startGameScript, Action onDone);
}

[RequireComponent(typeof(DungeonRoom))]
public class StartGameScript : MonoBehaviour
{
	[SerializeField]
	private StartGameTask[] startGameTasks = default;

	private int index = 0;

	// Start is called before the first frame update
	void Start()
	{
		if (startGameTasks.Length > 0)
			startGameTasks[0].Execute(this, OnTaskDone);
	}

	private void OnTaskDone()
	{
		++index;
		if (index < startGameTasks.Length)
			startGameTasks[index].Execute(this, OnTaskDone);
	}
}
