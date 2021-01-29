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
    private float moveSpeed = 5f;

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
        var input = move.ReadValue<Vector2>();

        characterController.Move((input * moveSpeed * Time.smoothDeltaTime).To3D());
    }
}
