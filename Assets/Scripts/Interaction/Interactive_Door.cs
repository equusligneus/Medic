using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive_Door : Interactive
{
	protected bool isMoving;

	protected bool isOpen;

	public override bool IsInteractive => !isMoving;

	protected override void Interact_Internal(InteractionComponent trigger)
	{
		if(!isOpen)
		{
			Open();
		}
		else
		{
			Close();
		}
	}

	protected abstract void Open();

	protected abstract void Close();
}
