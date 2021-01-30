using UnityEngine;
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

    void Awake()
	{
        camTarget = GetComponent<CameraTarget>();
        if (camTarget)
            camTarget.Activate();

        characterController = GetComponent<CharacterController>();
        if (!characterController)
            enabled = false;

        move.Enable();
	}

    // Update is called once per frame
    void Update()
    {
        // fainted, abort
        if (!isStanding.Get())
        {
            currentSpeed.Set(0f);
            return;
        }

        var input = move.ReadValue<Vector2>();

        var speed = this.speed * (isLoaded.Get() ? loadModifier : 1f);

        if (input.sqrMagnitude > 0f)
            transform.rotation = Quaternion.LookRotation(input.normalized.To3D(), Vector3.up);

        currentSpeed.Set(speed * input.magnitude);

        //characterController.Move((input * speed * Time.smoothDeltaTime).To3D());
        characterController.SimpleMove((input * speed).To3D());
    }
}
