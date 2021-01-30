using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSet<T> : ScriptableObject
{
	public event Action OnCountChanged;

	private List<T> set = new List<T>();

	public int Count
		=> set.Count;

	public T this[int index]
		=> set[index];
	public void Add(T item)
	{
		set.Add(item);
		OnCountChanged?.Invoke();
	}

	public void Remove(T item)
	{
		set.Remove(item);
		OnCountChanged?.Invoke();
	}

	public void RemoveAt(int index)
	{
		set.RemoveAt(index);
		OnCountChanged.Invoke();
	}
	public void Clear()
	{
		set.Clear();
		OnCountChanged?.Invoke();
	}
}
