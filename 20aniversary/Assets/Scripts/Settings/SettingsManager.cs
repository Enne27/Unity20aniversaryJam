using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Referencia al ScriptableObject con las configuraciones por defecto")]
    [SerializeField] private Settings settings;

    [Header("Audio Mixer (opcional)")]
    [SerializeField] private AudioMixer mainMixer;

    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";
    private const string MUTE_KEY = "MuteAll";
    private const string SENSITIVITY_KEY = "MouseSensitivity";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (settings == null)
        {
            Debug.LogWarning("No hay Settings asignado en SettingsManager.");
            return;
        }

        LoadSettingsFromPrefs();
        ApplyAudioSettings();

        Debug.Log("Configuraciones cargadas y aplicadas desde ScriptableSettings.");
    }

    public void ApplyAudioSettings()
    {
        if (settings.muteAll)
        {
            MuteAll(true);
            return;
        }

        if (mainMixer != null)
        {
            mainMixer.SetFloat("MasterVolume", LinearToDecibel(settings.masterVolume));
            mainMixer.SetFloat("MusicVolume", LinearToDecibel(settings.musicVolume));
            mainMixer.SetFloat("SFXVolume", LinearToDecibel(settings.sfxVolume));
        }
        else
        {
            AudioListener.volume = settings.masterVolume;
        }
    }

    public void SetMasterVolume(float value)
    {
        settings.masterVolume = Mathf.Clamp01(value);
        if (mainMixer != null)
            mainMixer.SetFloat("MasterVolume", LinearToDecibel(value));
        else
            AudioListener.volume = value;

        PlayerPrefs.SetFloat(MASTER_KEY, value);
    }

    public void SetMusicVolume(float value)
    {
        settings.musicVolume = Mathf.Clamp01(value);
        if (mainMixer != null)
            mainMixer.SetFloat("MusicVolume", LinearToDecibel(value));

        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    public void SetSFXVolume(float value)
    {
        settings.sfxVolume = Mathf.Clamp01(value);
        if (mainMixer != null)
            mainMixer.SetFloat("SFXVolume", LinearToDecibel(value));

        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    public void MuteAll(bool state)
    {
        settings.muteAll = state;

        if (state)
        {
            if (mainMixer != null)
            {
                mainMixer.SetFloat("MasterVolume", -80f);
                mainMixer.SetFloat("MusicVolume", -80f);
                mainMixer.SetFloat("SFXVolume", -80f);
            }
            else
            {
                AudioListener.volume = 0f;
            }
        }
        else
        {
            ApplyAudioSettings();
        }

        PlayerPrefs.SetInt(MUTE_KEY, state ? 1 : 0);
    }

    public void SetMouseSensitivity(float value)
    {
        settings.mouseCameraSensitivity = Mathf.Clamp(value, 0.01f, 1f);
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, settings.mouseCameraSensitivity);
    }

    public float GetMouseSensitivity()
    {
        return settings.mouseCameraSensitivity;
    }

    public void LoadSettingsFromPrefs()
    {
        if (PlayerPrefs.HasKey(MASTER_KEY))
            settings.masterVolume = PlayerPrefs.GetFloat(MASTER_KEY);

        if (PlayerPrefs.HasKey(MUSIC_KEY))
            settings.musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY);

        if (PlayerPrefs.HasKey(SFX_KEY))
            settings.sfxVolume = PlayerPrefs.GetFloat(SFX_KEY);

        if (PlayerPrefs.HasKey(MUTE_KEY))
            settings.muteAll = PlayerPrefs.GetInt(MUTE_KEY) == 1;

        if (PlayerPrefs.HasKey(SENSITIVITY_KEY))
            settings.mouseCameraSensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY);
    }

    public void SaveSettingsToPrefs()
    {
        PlayerPrefs.SetFloat(MASTER_KEY, settings.masterVolume);
        PlayerPrefs.SetFloat(MUSIC_KEY, settings.musicVolume);
        PlayerPrefs.SetFloat(SFX_KEY, settings.sfxVolume);
        PlayerPrefs.SetInt(MUTE_KEY, settings.muteAll ? 1 : 0);
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, settings.mouseCameraSensitivity);
        PlayerPrefs.Save();
    }

    private float LinearToDecibel(float linear)
    {
        return linear > 0 ? Mathf.Log10(linear) * 20f : -80f;
    }
}
