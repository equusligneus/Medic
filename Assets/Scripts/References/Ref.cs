using UnityEngine;

/// <summary>
/// Base class for referenced values. Read-only
/// </summary>
/// <typeparam name="T">The type of referenced value</typeparam>
public abstract class Ref<T> : ScriptableObject
{
    public abstract T Get();
}

/// <summary>
/// Reference that can be written to
/// </summary>
/// <typeparam name="T">The type of referenced value</typeparam>
public class WritableRef<T> : Ref<T>
{
	public void Set(T value)
		=> this.value = value;

	public override T Get()
		=> value;

	[System.NonSerialized]
	protected T value;
}

/// <summary>
/// Writable reference with a default value
/// </summary>
/// <typeparam name="T">The type of referenced value</typeparam>
public class WritableDefaultRef<T> : WritableRef<T>
{
	private void OnEnable()
		=> value = _value;

	[SerializeField]
	private T _value = default;
}