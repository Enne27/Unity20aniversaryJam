using UnityEngine;
using UnityEngine.InputSystem;

public class PickObject : MonoBehaviour
{
    public GameObject handPoint;
    private GameObject pickedObject = null;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference interact;

    private bool interactPressed = false;

    private void OnEnable()
    {
        interact.action.performed += OnInteractPerformed;
        interact.action.canceled += OnInteractCanceled;
        interact.action.Enable();
    }

    private void OnDisable()
    {
        interact.action.performed -= OnInteractPerformed;
        interact.action.canceled -= OnInteractCanceled;
        interact.action.Disable();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        interactPressed = true;

        if (pickedObject != null)
        {
            // Soltar objeto
            Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            pickedObject.transform.SetParent(null);
            pickedObject = null;
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        interactPressed = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CollectibleObject") && interactPressed && pickedObject == null)
        {
            // Recoger objeto
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            other.transform.position = handPoint.transform.position;
            other.transform.SetParent(handPoint.transform);
            pickedObject = other.gameObject;
        }
    }
}
