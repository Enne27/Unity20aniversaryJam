using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Referencia al ScriptableObject con las configuraciones por defecto")]
    [SerializeField] private Settings settings;

    [Header("Audio Mixer (opcional)")]
    [SerializeField] private AudioMixer mainMixer;

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

        ApplyScreenSettings();
        ApplyAudioSettings();
        Debug.Log("Configuraciones aplicadas desde ScriptableSettings.");
    }

    public void ApplyScreenSettings()
    {
        Vector2Int resolution = GetResolutionValue(settings.screenResolution);
        FullScreenMode mode = GetScreenMode(settings.screenMode);

        Screen.SetResolution(resolution.x, resolution.y, mode);
        QualitySettings.SetQualityLevel((int)settings.screenQuality, true);
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

    public void SetResolution(Settings.Resolution newRes)
    {
        settings.screenResolution = newRes;
        Vector2Int res = GetResolutionValue(newRes);
        Screen.SetResolution(res.x, res.y, GetScreenMode(settings.screenMode));
    }

    public void SetQuality(Settings.Quality newQuality)
    {
        settings.screenQuality = newQuality;
        QualitySettings.SetQualityLevel((int)newQuality, true);
    }

    public void SetScreenMode(Settings.ScreenMode newMode)
    {
        settings.screenMode = newMode;
        Vector2Int res = GetResolutionValue(settings.screenResolution);
        Screen.SetResolution(res.x, res.y, GetScreenMode(newMode));
    }

    public void SetMasterVolume(float value)
    {
        settings.masterVolume = Mathf.Clamp01(value);
        if (mainMixer != null)
            mainMixer.SetFloat("MasterVolume", LinearToDecibel(value));
        else
            AudioListener.volume = value;
    }

    public void SetMusicVolume(float value)
    {
        settings.musicVolume = Mathf.Clamp01(value);
        if (mainMixer != null)
            mainMixer.SetFloat("MusicVolume", LinearToDecibel(value));
    }

    public void SetSFXVolume(float value)
    {
        settings.sfxVolume = Mathf.Clamp01(value);
        if (mainMixer != null)
            mainMixer.SetFloat("SFXVolume", LinearToDecibel(value));
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
    }

    private float LinearToDecibel(float linear)
    {
        return linear > 0 ? Mathf.Log10(linear) * 20f : -80f;
    }

    private Vector2Int GetResolutionValue(Settings.Resolution res)
    {
        return res switch
        {
            Settings.Resolution._3840x2160 => new Vector2Int(3840, 2160),
            Settings.Resolution._2560x1440 => new Vector2Int(2560, 1440),
            Settings.Resolution._1920x1080 => new Vector2Int(1920, 1080),
            Settings.Resolution._1280x720 => new Vector2Int(1280, 720),
            Settings.Resolution._854x480 => new Vector2Int(854, 480),
            _ => new Vector2Int(1920, 1080)
        };
    }

    private FullScreenMode GetScreenMode(Settings.ScreenMode mode)
    {
        return mode switch
        {
            Settings.ScreenMode.Fullscreen => FullScreenMode.ExclusiveFullScreen,
            Settings.ScreenMode.Borderless => FullScreenMode.FullScreenWindow,
            Settings.ScreenMode.Windowed => FullScreenMode.Windowed,
            _ => FullScreenMode.Windowed
        };
    }
}
