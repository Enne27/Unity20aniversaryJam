using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    #region SINGLETON
    static MusicManager musicManager;

    public static MusicManager instance
    {
        get
        {
            return RequestMusicManager();
        }
    }

    private static MusicManager RequestMusicManager()
    {
        if (musicManager == null)
        {
            musicManager = FindAnyObjectByType<MusicManager>();
        }
        return musicManager;
    }
    #endregion

    #region VARIABLES
    [Header("Audio Clips de Música por Escena")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioMixer mixer;

    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscripci�n al cargar la escena.
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Debe eliminarse la subscripci�n para que no se llame m�s de una vez.
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        AudioClip nextClip = null;

        switch (scene.name)
        {
            case "GameScene":
                nextClip = gameMusic;
                break;
            default:
                nextClip = mainMenuMusic;
            break;
        }

        if (nextClip != null)
            StartCoroutine(SwitchMusic(nextClip, 1.0f));
    }

    private IEnumerator SwitchMusic(AudioClip newClip, float fadeDuration) //   TRADUCIDO DE FMOD A AUDIO SOURCE, SI NO VA, FUERA
    {
        // Fade out
        float startVolume = musicSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        StopMusic();
        musicSource.clip = newClip;
        PlayMusic();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    #region MUSIC_CONTROL
    public void PlayMusic() => musicSource?.Play();
    public void StopMusic() => musicSource?.Stop();
    public void PauseMusic() => musicSource?.Pause();
    public void ResumeMusic() => musicSource?.UnPause();
    #endregion


    public void SetMusicVolume(float volume)
    {
        // volume debe estar entre 0.0001f y 1.0f para evitar log(0)
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        mixer.SetFloat("MusicVolume", dB);
    }
}
