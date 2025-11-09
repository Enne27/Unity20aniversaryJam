using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreditsCanvas : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] Image logo;
    [Header("BotonesEscena")]
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;

    [Header("Efectos")]
    [SerializeField] private CanvasGroup CanvasGroupButtons;
    [SerializeField, Range(0.1f, 5f), Tooltip("Tiempo inactividad para ocultar (segundos)")]
    private float mouseInactivityTime = 2f;
    [SerializeField, Range(0.1f, 2f), Tooltip("Duraci�n del fade del bot�n")]
    private float buttonFadeDuration = 0.3f;
    [SerializeField, Range(1f, 10f), Tooltip("Umbral movimiento del rat�n (p�xeles)")]
    private float mouseMoveThreshold = 3f;

    private Vector3 lastMousePos;
    private float inactivityTimer;
    private bool isCanvasGroupVisible;
    private Coroutine fadeCoroutine;

    [SerializeField, Tooltip("Tiempo hasta mostrar los botones al final")] private float showButtonsDelay = 5f;
    private bool ignoreMouse = false;
    #endregion

    private void OnEnable()
    {
        if (CanvasGroupButtons == null)
            CanvasGroupButtons = buttonsParent.GetComponent<CanvasGroup>() ?? buttonsParent.gameObject.AddComponent<CanvasGroup>();

        CanvasGroupButtons.alpha = 0f;
        isCanvasGroupVisible = false;
        ignoreMouse = false;

        InitializeButtons(exitButton, ScenesManager.Instance.ExitGame);
        InitializeButtons(restartButton, () => ScenesManager.Instance.ChangeScene("MainMenuScene"));

        StartCoroutine(ShowButtonsAfterDelay());
    }

    private IEnumerator ShowButtonsAfterDelay()
    {
        yield return new WaitForSeconds(showButtonsDelay);
        yield return StartCoroutine(FadeButtons(true));
        ignoreMouse = true;
    }

    private void InitializeButtons(Button button, UnityAction onClickAction)
    {
        button.interactable = false;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClickAction);

        lastMousePos = Vector3.zero;
    }

    private void Update()
    {
        if (ignoreMouse)
        {
            logo.gameObject.SetActive(true);
            return;
        }
        HandleMouseActivity();
    }

    private void HandleMouseActivity()
    {
        Vector3 currentMousePos = Input.mousePosition;

        // Detecci�n de movimiento
        if (Vector3.Distance(currentMousePos, lastMousePos) > mouseMoveThreshold)
        {
            HandleMouseMovement();
        }
        else
        {
            HandleMouseInactivity();
        }

        lastMousePos = currentMousePos;
    }

    private void HandleMouseMovement()
    {
        inactivityTimer = 0f;

        if (!isCanvasGroupVisible && fadeCoroutine == null)
        {
            fadeCoroutine = StartCoroutine(FadeButtons(true));
        }
    }

    private void HandleMouseInactivity()
    {
        inactivityTimer += Time.unscaledDeltaTime;

        if (inactivityTimer >= mouseInactivityTime && isCanvasGroupVisible && fadeCoroutine == null)
        {
            fadeCoroutine = StartCoroutine(FadeButtons(false));
        }
    }


    private IEnumerator FadeButtons(bool show)
    {
        float startAlpha = CanvasGroupButtons.alpha;
        float targetAlpha = show ? 1f : 0f;
        float elapsed = 0f;

        while (elapsed < buttonFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            CanvasGroupButtons.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / buttonFadeDuration);
            yield return null;
        }

        CanvasGroupButtons.alpha = targetAlpha;

        foreach (Button btn in buttonsParent.GetComponentsInChildren<Button>(true))
        {
            btn.interactable = show;
        }

        isCanvasGroupVisible = show;

        fadeCoroutine = null;
    }

}