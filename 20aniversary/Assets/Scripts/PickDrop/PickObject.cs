using UnityEngine;
using UnityEngine.InputSystem;

public class PickObject : MonoBehaviour
{
    public GameObject handPoint;
    private GameObject pickedObject = null;
    [Header("Input Actions")]
    [SerializeField] InputActionReference interact;
    void Update()
    {
        if (pickedObject != null)
        {
            if (interact.action.IsPressed())
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.gameObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CollectibleObject"))
        {
            if (interact.action.IsPressed() && pickedObject == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = handPoint.transform.position;
                other.gameObject.transform.SetParent(handPoint.gameObject.transform);
                pickedObject = other.gameObject;
            }
        }
    }
    void OnEnable()
    {
        interact.action.Enable();
    }

    void OnDisable()
    {
        interact.action.Disable();
    }
}
