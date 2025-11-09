using Autodesk.Fbx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CajonInteract : MonoBehaviour
{
    [Header("References")]
    public GameObject handPoint;
    private GameObject objectInRange = null;

    [Header("Cursor Objects")]
    public GameObject offObjectCursor;
    public GameObject onObjectCursor;

    [Header("Input Action")]
    [SerializeField] private InputActionReference interact;

    [Header("Interact hint")]
    [SerializeField] private GameObject canvasInteractHint;

    [Header("Animator Trigger Name")]
    [SerializeField] private string animationTrigger = "Interact";

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
        if (objectInRange == null) return;

        Animator anim = objectInRange.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger(animationTrigger);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cajon"))
        {
            objectInRange = other.gameObject;
            onObjectCursor.SetActive(true);
            offObjectCursor.SetActive(false);
            canvasInteractHint.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cajon"))
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
