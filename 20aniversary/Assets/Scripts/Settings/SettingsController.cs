using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Sliders de Volumen")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Toggle de Mute")]
    [SerializeField] private Toggle muteToggle;

    [Header("Slider de Sensibilidad del Ratón")]
    [SerializeField] private Slider sensitivitySlider;

    private void Start()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("No existe un SettingsManager en la escena.");
            return;
        }

        InitializeUI();
        AddListeners();
    }

    private void InitializeUI()
    {
        var manager = SettingsManager.Instance;
        var settings = GetSettings();

        if (masterSlider != null)
            masterSlider.value = settings.masterVolume;

        if (musicSlider != null)
            musicSlider.value = settings.musicVolume;

        if (sfxSlider != null)
            sfxSlider.value = settings.sfxVolume;

        if (muteToggle != null)
            muteToggle.isOn = settings.muteAll;

        if (sensitivitySlider != null)
            sensitivitySlider.value = settings.mouseCameraSensitivity;
    }

    private void AddListeners()
    {
        var manager = SettingsManager.Instance;

        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(value => manager.SetMasterVolume(value));

        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(value => manager.SetMusicVolume(value));

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(value => manager.SetSFXVolume(value));

        if (muteToggle != null)
            muteToggle.onValueChanged.AddListener(state => manager.MuteAll(state));

        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.AddListener(value => manager.SetMouseSensitivity(value));
    }

    private Settings GetSettings()
    {
        return SettingsManager.Instance != null ?
            typeof(SettingsManager)
                .GetField("settings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(SettingsManager.Instance) as Settings
            : null;
    }

    // Llamar este método desde un botón de “Guardar” si quieres persistir los cambios manualmente
    public void SaveSettings()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SaveSettingsToPrefs();
            Debug.Log("Configuración guardada.");
        }
    }
}
