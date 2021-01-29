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
        if (!isAwake.Get())
            return;

        var input = move.ReadValue<Vector2>();

        var speed = this.speed * (isLoaded.Get() ? loadModifier : 1f); 

        characterController.Move((input * speed * Time.smoothDeltaTime).To3D());
    }
}
