using UnityEngine;
using UnityEngine.InputSystem;

public class TimeShiftManager : MonoBehaviour
{
    [SerializeField] InputActionReference timeTravelAction;

    //El nivel empieza en el presente
    public bool isPresent = true;
    public GameObject player;
    public Camera playerCam;
    private LayerMask presentLayer;
    private LayerMask pastLayer;

    /// <summary>
    /// En start, buscamos al player, y recuperamos su cámara.
    /// Buscamos las layers de pasado y presente, para poder uasrlas luego
    /// </summary>
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerCam = Camera.main;
        pastLayer = LayerMask.NameToLayer("Past");
        presentLayer = LayerMask.NameToLayer("Present");
    }

    private void OnEnable()
    {
        timeTravelAction.action.Enable();
    }

    private void OnDisable()
    {
        timeTravelAction.action.Disable();
    }

    void Update()
    {
        if (timeTravelAction.action.IsPressed())
        {
            ShiftTime();
        }
    }

    /// <summary>
    /// Al ejecutar la función, el script mira si la variable isPresent está activada,
    /// si lo está, cambia la CullingMask de la cámara activando la del pasado, 
    /// desactivando la del presente, y cambiando isPresent a false, en caso de que
    /// isPresent sea false, hace la operación inversa
    /// </summary>
    private void ShiftTime()
    {
        if (isPresent)
        {
            playerCam.cullingMask &= (1 << pastLayer);
            playerCam.cullingMask |= ~(1 << presentLayer);
            player.layer = pastLayer;
            isPresent = false;
        }
        else
        {
            playerCam.cullingMask &= (1 << presentLayer);
            playerCam.cullingMask |= ~(1 << pastLayer);
            player.layer = presentLayer;
            isPresent = true;
        }
    }
}
