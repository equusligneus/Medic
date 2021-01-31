using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactive))]
public class SelectionOutlineScript : MonoBehaviour
{
	[SerializeField]
	private cakeslice.Outline outline = default;

	private void OnEnable()
	{
		var trigger = GetComponent<Interactive>();
		trigger.OnSelected += OnSelection;
	}

	private void OnDisable()
	{
		var trigger = GetComponent<Interactive>();
		trigger.OnSelected -= OnSelection;
	}

	private void OnSelection(bool showOutline)
	{
		outline.enabled = showOutline;
	}
}
