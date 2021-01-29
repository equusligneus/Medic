using UnityEngine;

public class CameraTarget : MonoBehaviour
{
	[SerializeField]
	private Ref_CameraTarget activeTarget = default;

	public Vector2 Position
		=> transform.position.To2D();

	public void Activate()
	{
		if (activeTarget) activeTarget.Set(this);
	}
}
