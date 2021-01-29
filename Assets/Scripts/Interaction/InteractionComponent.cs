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
	private InputAction interact = default;


	private List<Interactive> interactives = new List<Interactive>();

	private bool canInteract = true;

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
		Interactive temp = selected.Get();
		if (!canInteract || (temp && !temp.IsInteractive))
			temp = default;

		int prio = temp ? temp.Priority : -1;

		if (canInteract)
		{
			foreach (var selectable in interactives)
			{
				if (selectable.IsInteractive && selectable.Priority > prio)
				{
					temp = selectable;
					prio = selectable.Priority;
				}
			} 
		}

		if (temp != selected.Get())
		{
			if (selected)
				selected.Get().Deselect();

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
			selected.Get().Interact(this);
	}



}
