using System;
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

	public bool IsRescued { get; private set; }

	public event Action OnRescue;

	[SerializeField]
	private GameObject _modle;

	private void OnEnable()
	{
        adventurers.Add(this);
	}

	private void OnDisable()
	{
        adventurers.Remove(this);
	}

	public void GotRescued()
    {
		IsRescued = true;
        OnRescue.Invoke();
		_modle.SetActive(false);
    }
}
