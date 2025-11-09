using Autodesk.Fbx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickObject : MonoBehaviour
{
    [Header("References")]
    public GameObject handPoint;
    private GameObject pickedObject = null;
    private GameObject objectInRange = null;

    [Header("Cursor Objects")]
    public GameObject offObjectCursor;
    public GameObject onObjectCursor;

    [Header("Input Action")]
    [SerializeField] private InputActionReference interact;

    [Header("Interact hint")]
    [SerializeField] private GameObject canvasInteractHint;

    private void OnEnable()
    {
        interact.action.performed += OnInteract;
        interact.action.Enable();
    }

    private void OnDisable()
    {
        interact.action.performed -= OnInteract;
        interact.action.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Si no tenemos nada en la mano y hay un objeto en rango → recoger
        if (pickedObject == null && objectInRange != null)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            objectInRange.transform.position = handPoint.transform.position;
            objectInRange.transform.SetParent(handPoint.transform);
            pickedObject = objectInRange;
        }
        // Si ya tenemos un objeto → soltar
        else if (pickedObject != null)
        {
            Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            pickedObject.transform.SetParent(null);
            pickedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectibleObject"))
        {
            objectInRange = other.gameObject;
            onObjectCursor.SetActive(true);
            offObjectCursor.SetActive(false);
            canvasInteractHint.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CollectibleObject"))
        {
            if (other.gameObject == objectInRange)
            {
                objectInRange = null;
                onObjectCursor.SetActive(false);
                offObjectCursor.SetActive(true);
                canvasInteractHint.SetActive(false);
            }
        }
    }
}
