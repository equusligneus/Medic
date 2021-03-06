﻿using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CameraTarget), typeof(CharacterController))]
public class MedicController : MonoBehaviour
{
    private CameraTarget camTarget = default;
    private CharacterController characterController = default;

    [SerializeField]
    private InputAction move = default;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float loadModifier = .75f;

    [SerializeField]
    private Ref<bool> isLoaded = default;

    [SerializeField]
    private Ref<bool> isAwake = default;

    [SerializeField]
    private Ref<bool> isStanding = default;

    [SerializeField]
    private Ref_Float currentSpeed = default;

    [SerializeField]
    private Ref_Transform medic = default;

    void Awake()
	{
        camTarget = GetComponent<CameraTarget>();
        if (camTarget)
            camTarget.Activate();

        characterController = GetComponent<CharacterController>();
        if (!characterController)
            enabled = false;

        medic.Set(transform);

        move.Enable();
	}

    // Update is called once per frame
    void Update()
    {
        var input = isStanding.Get() ? move.ReadValue<Vector2>() : Vector2.zero;

        var speed = this.speed * (isLoaded.Get() ? loadModifier : 1f);

        if (input.sqrMagnitude > 0f)
            transform.rotation = Quaternion.LookRotation(input.normalized.To3D(), Vector3.up);

        currentSpeed.Set(speed * input.magnitude);

        characterController.SimpleMove((input * speed).To3D());
    }
}
