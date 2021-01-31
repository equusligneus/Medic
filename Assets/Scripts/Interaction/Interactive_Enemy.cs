using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_Enemy : Interactive
{
	public override bool IsInteractive => throw new System.NotImplementedException();

	public override InteractionType type => InteractionType.Punch;

	protected override void Interact_Internal(InteractionComponent trigger)
	{
		throw new System.NotImplementedException();
	}
}
