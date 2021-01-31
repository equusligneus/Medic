using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionComponent : MonoBehaviour
{
	[SerializeField]
	private Ref_Bool isInteracting = default;

	[SerializeField]
	private Ref_Interactive selected = default;

	[SerializeField]
	private Ref_Bool isPunching = default;

	[SerializeField]
	private InputAction interact = default;

	[SerializeField]
	private Ref<bool> isAwake = default;

	[SerializeField]
	private Ref<Interactive> draggedObject = default; 

	private List<Interactive> interactives = new List<Interactive>();

	void Awake()
	{
		interact.performed += OnInteract;
		interact.Enable();
	}

	// Update is called once per frame
	void Update()
	{
		if (isInteracting.Get())
		{
			UpdateInteraction();
			return;
		}

		UpdateSelection();
	}

	void OnTriggerEnter(Collider collider)
	{
		var interactive = collider.GetComponent<Interactive>();

		if (interactive)
			interactives.Add(interactive);
	}

	void OnTriggerExit(Collider collider)
	{
		var interactive = collider.GetComponent<Interactive>();

		if (!interactive)
			return;

		interactives.Remove(interactive);

		if (selected.Get() == interactive)
		{
			if (!isInteracting.Get())
			{
				interactive.Deselect();
				selected.Set(default);
			}
		}
	}

	void OnDestroy()
	{
		interact.performed -= OnInteract;
	}

	private void UpdateSelection()
	{
		Interactive selection = selected.Get();
		Interactive temp = selection;
		if (!isAwake.Get() || !CanInteract(temp))
			temp = default;

		if (isAwake.Get())
		{
			int prio = temp ? temp.Priority : -1;

			foreach (var selectable in interactives)
			{
				if (CanInteract(selectable) && selectable.Priority > prio)
				{
					temp = selectable;
					prio = selectable.Priority;
				}
			} 
		} 

		if (temp != selection)
		{
			if (selection)
				selection.Deselect();

			selected.Set(temp);
			if (temp)
				temp.Select();
		}
	}

	private void UpdateInteraction()
	{
		// do something, if needed, maybe
	}

	private void OnInteract(InputAction.CallbackContext context)
	{
		if (!selected.Get())
			return;

		if (!isInteracting.Get())
		{
			if(selected.Get().type == InteractionType.Punch)
				isPunching.Set(true);

			selected.Get().Interact(this);
			
		}
	}

	private bool CanInteract(Interactive interactive)
	{
		if (!interactive || !interactive.IsInteractive)
			return false;

		if (draggedObject.Get() && draggedObject.Get() != interactive)
			return false;

		return true;
	}

}
