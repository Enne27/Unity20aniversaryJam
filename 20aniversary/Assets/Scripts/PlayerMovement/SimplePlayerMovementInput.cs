using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovementInput : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 9f;

    [Header("Input Actions")]
    [SerializeField] InputActionReference moveAction;   
    [SerializeField] InputActionReference sprintAction; 

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        bool isSprinting = sprintAction.action.IsPressed();

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        sprintAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        sprintAction.action.Disable();
    }
}
