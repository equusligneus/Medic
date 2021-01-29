using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
	[SerializeField]
	private int priority = 0;

	public abstract bool IsInteractive { get; }

	public int Priority => priority;

	public event Action<bool> OnSelected = default;

	public void Select()
		=> OnSelected?.Invoke(true);

	public void Deselect()
		=> OnSelected?.Invoke(false);

	public abstract void Interact(InteractionComponent trigger);
}
