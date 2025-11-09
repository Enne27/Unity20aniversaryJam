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

<<<<<<< HEAD
=======
    // Fade Variables
    private float fadeDuration = 1f;
    private float blackExposure = -10f;
    private bool isFading = false;
    private bool isFadingBack = false;
    private float currentFadeTime = 0f;

>>>>>>> origin/Anuk
    /// <summary>
    /// En start, buscamos al player, y recuperamos su c�mara.
    /// Buscamos las layers de pasado y presente, para poder uasrlas luego
    /// </summary>
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pastLayer = LayerMask.NameToLayer("Past");
        presentLayer = LayerMask.NameToLayer("Present");
        playerCam.cullingMask &= (1 << presentLayer);
        playerCam.cullingMask |= ~(1 << pastLayer);
        player.layer = presentLayer;

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
<<<<<<< HEAD
        if (timeTravelAction.action.WasPressedThisFrame())
=======
        if (timeTravelAction.action.WasPressedThisFrame() && !timeWarpVFX.isPlaying)
>>>>>>> origin/Anuk
        {
            Debug.Log("Pressed");
            ShiftTime();
        }
    }

    /// <summary>
    /// Al ejecutar la funci�n, el script mira si la variable isPresent est� activada,
    /// si lo est�, cambia la CullingMask de la c�mara activando la del pasado, 
    /// desactivando la del presente, y cambiando isPresent a false, en caso de que
    /// isPresent sea false, hace la operaci�n inversa
    /// </summary>
    private void ShiftTime()
    {
        if (isPresent)
        {
            playerCam.cullingMask &= (1 << pastLayer);
            playerCam.cullingMask |= ~(1 << presentLayer);
            player.layer = pastLayer;
            isPresent = false;
            // MusicManager.Instance.PlayMusic();
        }
        else
        {
            playerCam.cullingMask &= (1 << presentLayer);
            playerCam.cullingMask |= ~(1 << pastLayer);
            player.layer = presentLayer;
            isPresent = true;
            // MusicManager.Instance.PlayMusic();
        }
    }
}
