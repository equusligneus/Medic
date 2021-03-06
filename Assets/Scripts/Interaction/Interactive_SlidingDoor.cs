﻿using UnityEngine;

public class Interactive_SlidingDoor : Interactive_Door
{
	[SerializeField]
	private Transform doorObject;

	private Vector3 closedPos = default;

	[SerializeField]
	private Vector3 offset = default;

	[SerializeField, Range(0.1f, 15f)]
	private float moveDuration = 1f;

	private float current = 0f;

	[SerializeField]
	private Collider doorCollider;

	private void Start()
	{
		if(!doorObject)
		{
			enabled = false;
			return;
		}
		closedPos = doorObject.transform.position;
	}

	void Update()
	{
		if (!isMoving)
			return;

		var offset = doorObject.transform.rotation * this.offset;

		var startPosition = isOpen ? closedPos + offset : closedPos;
		var endPosition = isOpen ? closedPos : closedPos + offset;

		current += Time.smoothDeltaTime;

		doorObject.transform.position = Vector3.Slerp(startPosition, endPosition, current / moveDuration);
		
		if(current >= moveDuration)
		{
			isMoving = false;
			isOpen = !isOpen;
			doorCollider.enabled = !isOpen;
		}
	}

	protected override void Close()
	{
		isMoving = true;
		current = 0f;
		doorCollider.enabled = true;
	}

	protected override void Open()
	{
		isMoving = true;
		current = 0f;
	}


}
