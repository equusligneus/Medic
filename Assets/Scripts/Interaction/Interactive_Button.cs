using UnityEngine;

public class Interactive_Button : Interactive
{
    [SerializeField]
    private Interactive[] targets = default;

	public override bool IsInteractive
    {
        get
		{
            foreach(var target in targets)
			{
                if (target && !target.IsInteractive)
                    return false;
			}
            return true;
		}
    }

	protected override void Interact_Internal(InteractionComponent trigger)
	{
        foreach (var target in targets)
            target.Interact(trigger);
	}
}
