using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Ref<CameraTarget> activeTarget = default;

    [SerializeField]
    private float maxSpeed = 15f;

    [SerializeField, Range(0f, 1f)]
    private float lerpFactor = .9f;

    // Update is called once per frame
    void Update()
    {
        // no targets, abort
        if (!activeTarget || !activeTarget.Get())
            return;

        var target = activeTarget.Get().Position;

        var delta = target - new Vector2(transform.position.x, transform.position.z);
        var distance2 = delta.sqrMagnitude;

        // on point, abort
        if (distance2 <= 0f)
            return;

        var direction = delta.normalized;

        delta = direction * Mathf.Clamp(Mathf.Sqrt(distance2) * lerpFactor, 0, maxSpeed * Time.smoothDeltaTime);

        transform.Translate(delta.To3D(), Space.World);
    }
}
