using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    [SerializeField]
    private Set_Adventurer adventurers = default;

	[SerializeField]
	private Sprite icon = default;

	public Sprite Icon => icon;

	private void OnEnable()
	{
        adventurers.Add(this);
	}

	private void OnDisable()
	{
        adventurers.Remove(this);
	}
}
