using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive_Door : Interactive
{
	protected bool isMoving;

	protected bool isOpen;

	public override bool IsInteractive => !isMoving;

	public override void Interact(InteractionComponent trigger)
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
