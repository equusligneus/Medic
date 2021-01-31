using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_Adventurer : Interactive
{
	[SerializeField]
	private Ref_Bool isDragging = default;

	private Transform anchor = default;

	[SerializeField]
	private Transform pivot = default;

	[SerializeField]
	private Transform dragPoint = default;

	[SerializeField]
	private float minDistance = 0.9f;

	[SerializeField]
	private float maxDistance = 1f;

	[SerializeField]
	private Ref_Interactive draggedAdventurer = default;

	public override bool IsInteractive => true;


	protected override void Interact_Internal(InteractionComponent trigger)
	{
		isDragging.Set(!isDragging.Get());
		draggedAdventurer.Set(isDragging.Get() ? this : default);
		anchor = trigger.transform.parent;
	}

	private void Start()
	{
		GetComponentInParent<Adventurer>().OnRescue += OnRescue;
	}

	private void OnRescue()
	{
		// force drop
		draggedAdventurer.Set(default);
		isDragging.Set(false);
	}

	private void Update()
	{
		// just lazing around...
		if (!isDragging.Get() || draggedAdventurer.Get() != this)
			return;

		var delta = (anchor.position - dragPoint.position).To2D();

		var direction = delta.normalized;
		var distance = Mathf.Clamp(delta.magnitude, minDistance, maxDistance);

		var targetDragPoint = anchor.position.To2D() - direction * distance;

		var pivToDrag = (dragPoint.position - pivot.position).To2D();
		var pivToDragDistance = pivToDrag.magnitude;
		var pivToNewDragDir = (targetDragPoint - pivot.position.To2D()).normalized;

		pivot.rotation = Quaternion.LookRotation(pivToNewDragDir.To3D(), Vector3.up);

		var dragDelta = (targetDragPoint - pivToDragDistance * pivToNewDragDir) - pivot.position.To2D();
		pivot.position += dragDelta.To3D();
	}
}
