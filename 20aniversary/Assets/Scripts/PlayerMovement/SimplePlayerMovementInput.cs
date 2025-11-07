using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovementInput : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] InputActionReference moveAction;

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);
    }

    void OnEnable() => moveAction.action.Enable();
    void OnDisable() => moveAction.action.Disable();
}
