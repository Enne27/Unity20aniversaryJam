using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeShiftManager : MonoBehaviour
{
    [SerializeField] InputActionReference timeTravelAction;

    [SerializeField] ParticleSystem timeWarpVFX;

    //PostProcessing
    [SerializeField] VolumeProfile postProcessProfile;
    private ColorAdjustments colorAdjustments;

    // El nivel empieza en el presente
    public bool isPresent = true;
    public GameObject player;
    public Camera playerCam;
    private LayerMask presentLayer;
    private LayerMask pastLayer;

    public static bool hasDispositivoTemporal = false;

    // Fade Variables
    private float fadeDuration = 1f;
    private float targetExposure = 0f;
    private float blackExposure = -10f;
    private bool isFading = false;
    private bool isFadingBack = false;
    private float currentFadeTime = 0f;

    /// <summary>
    /// En start, buscamos al player, y recuperamos su c�mara.
    /// Buscamos las layers de pasado y presente, para poder usarlas luego
    /// </summary>
    void Start()
    {
        postProcessProfile.TryGet<ColorAdjustments>(out colorAdjustments);
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
        if (timeTravelAction.action.WasPressedThisFrame() && hasDispositivoTemporal && !timeWarpVFX.isPlaying)
        {
            ShiftTime();
        }

        if (isFading)
        {
            currentFadeTime += Time.deltaTime;
            float fadeValue = Mathf.Lerp(0f, blackExposure, currentFadeTime / fadeDuration);
            colorAdjustments.postExposure.value = fadeValue;

            if (currentFadeTime >= fadeDuration)
            {
                if (isPresent)
                {
                    // Switch to past
                    playerCam.cullingMask &= (1 << pastLayer);
                    playerCam.cullingMask |= ~(1 << presentLayer);
                    player.layer = pastLayer;
                    isPresent = false;
                }
                else
                {
                    // Switch to present
                    playerCam.cullingMask &= (1 << presentLayer);
                    playerCam.cullingMask |= ~(1 << pastLayer);
                    player.layer = presentLayer;
                    isPresent = true;
                }
                isFading = false;
                isFadingBack = true;
                currentFadeTime = 0f;
            }
        }

        if (isFadingBack)
        {
            currentFadeTime += Time.deltaTime;
            float fadeBackValue = Mathf.Lerp(blackExposure, 0f, currentFadeTime / fadeDuration);
            colorAdjustments.postExposure.value = fadeBackValue;

            if (currentFadeTime >= fadeDuration)
            {
                isFadingBack = false;
                colorAdjustments.postExposure.value = 0f;
            }
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
        timeWarpVFX.Play();

        colorAdjustments.postExposure.value = 0f;
        isFading = true;
        currentFadeTime = 0f;
                if (isPresent)
        {
            playerCam.cullingMask &= (1 << pastLayer);
            playerCam.cullingMask |= ~(1 << presentLayer);
            player.layer = pastLayer;
            isPresent = false;
            // // MusicManager.Instance.ChangeMusicTime(false);
        }
        else
        {
            playerCam.cullingMask &= (1 << presentLayer);
            playerCam.cullingMask |= ~(1 << pastLayer);
            player.layer = presentLayer;
            isPresent = true;
            // MusicManager.Instance.ChangeMusicTime(true);
        }
    }
}
