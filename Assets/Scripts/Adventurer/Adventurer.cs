﻿using System;
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

	private void OnEnable()
	{
        adventurers.Add(this);
	}

	private void OnDisable()
	{
        adventurers.Remove(this);
	}
}
