using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_Enemy : Interactive
{
	[SerializeField]
	private KIController controller = default;

	public override bool IsInteractive => controller.CanBeStunned;

	public override InteractionType type => InteractionType.Punch;

	protected override void Interact_Internal(InteractionComponent trigger)
	{
		if (controller.CanBeStunned)
			controller.Stun();
	}
}
