using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Game Settings/New Game Settings")]
public class ScriptableSettings : ScriptableObject
{
    [Header("Screen Settings")]
    [SerializeField, Tooltip("Cambia la resolucion de la pantalla del juego")] public Resolution screenResolution;
    [SerializeField, Tooltip("Cambia la calidad grafica del juego")] public Quality screenQuality;
    [SerializeField, Tooltip("Cambia el modo de ventana del juego")] public ScreenMode screenMode;

    [Header("Audio Settings")]
    [SerializeField, Range(0,1)] public float masterVolume = 1; 
    [SerializeField, Range(0,1)] public float musicVolume = 1; 
    [SerializeField, Range(0,1)] public float sfxVolume = 1;

    public enum Resolution { _3840x2160, _2560x1440, _1920x1080, _1280x720, _854x480 }
    public enum Quality {Ultra, High, Medium, Low }
    public enum ScreenMode {Fullscreen, Borderless, Windowed}
}
