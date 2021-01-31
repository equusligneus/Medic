using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
	None,
	Punch
}

public abstract class Interactive : MonoBehaviour
{
	[SerializeField]
	private int priority = 0;

	public abstract bool IsInteractive { get; }

	public virtual InteractionType type => InteractionType.None;

	public int Priority => priority;

	public event Action<bool> OnSelected = default;

	public event Action OnInteracted = default;

	public void Select()
		=> OnSelected?.Invoke(true);

	public void Deselect()
		=> OnSelected?.Invoke(false);

	public void Interact(InteractionComponent trigger)
	{
		Interact_Internal(trigger);
		OnInteracted?.Invoke();
	}

	protected abstract void Interact_Internal(InteractionComponent trigger);
}
