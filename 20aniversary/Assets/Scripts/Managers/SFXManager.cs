using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    #region VARIABLES

    static SFXManager sfxManager;

    List<AudioSource> sfxEventEmitters = new List<AudioSource>(); // Pueden haber varios SFX al mismo tiempo.
    float cleanupTimer = 1.0f; // Contadores para limpiar la lista de sonidos.
    float cleanupEvery = 1.0f;

    [Header("Mixer de Audio")]
    [SerializeField] private AudioMixer audioMixer;

    #endregion 

    /// <summary>
    /// Obtener la instancia de SFXmanager.
    /// </summary>
    public static SFXManager instance
    {
        get
        {
            return RequestSFXManager();
        }
    }

    /// <summary>
    /// Comprobar que ni SFXmanager ni la lista de sonidos sean nulos.
    /// </summary>
    /// <returns></returns>
    static SFXManager RequestSFXManager()
    {
        if (!sfxManager)
        {
            SetSFXManagerInstance(FindFirstObjectByType<SFXManager>());
        }

        return sfxManager;
    }

    static void InitializeSFXManager()
    {
        if (sfxManager == null) return;
        if (sfxManager.sfxEventEmitters == null)
        {
            sfxManager.sfxEventEmitters = new List<AudioSource>();
        }
    }

    static void SetSFXManagerInstance(SFXManager anInstance)
    {
        sfxManager = anInstance;
        InitializeSFXManager();

    }

    private void Awake()
    {
        if (sfxManager == null)
        {
            SetSFXManagerInstance(this);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Hacemos sonar el SFX que queramos y lo a�adimos a la lista si no estaba.
    /// </summary>
    /// <param name="source"></param>
    public static void PlaySFX(AudioSource source)
    {
        if (sfxManager == null) RequestSFXManager();

        if (!instance.sfxEventEmitters.Contains(source))
        {
            sfxManager.sfxEventEmitters.Add(source);
        }
        source.Play();
    }

    /// <summary>
    /// Detener el SFX que est� sonando y eliminarlo de nuestra lista.
    /// </summary>
    /// <param name="source"></param>
    public static void StopSFX(AudioSource source)
    {
        if (instance.sfxEventEmitters.Contains(source))
        {
            sfxManager.sfxEventEmitters.Remove(source);
        }
        source.Stop();
    }

    /// <summary>
    /// Silenciar todos los SFX a la vez.
    /// </summary>
    public static void StopAllSFX()
    {
        foreach (AudioSource e in instance.sfxEventEmitters)
        {
            e.Stop();
        }

    }

    /// <summary>
    /// Pausar el sonido de cada SFX que est� en la lista en ese momento.
    /// </summary>
    public static void Pause()
    {
        foreach (AudioSource e in instance.sfxEventEmitters)
        {
            e.Pause();
        }
    }

    /// <summary>
    /// Volver a hacer sonar los SFX que est�n en la lista.
    /// </summary>
    public static void Resume()
    {
        if (instance.sfxEventEmitters.Count > 0)
        {
            foreach (AudioSource e in sfxManager.sfxEventEmitters)
            {
                if (e != null) e.UnPause();
            }
        }
    }

    private void Update()
    {
        if (cleanupTimer >= cleanupEvery) // Comprobamos cada cierto tiempo para borrar los SFX que se hayan quedado colgados all�.
        {
            CleanUp();
        }
        else
        {
            cleanupTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Eliminamos los SFX que est�n en la lista porque no le hayamos dado a Stop cuando hayan terminado.
    /// </summary>
    private void CleanUp()
    {
        sfxEventEmitters.RemoveAll((AudioSource e) => e == null || !e.isPlaying);
    }


    public void SetSFXVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFXVolume", dB);
    }

}
