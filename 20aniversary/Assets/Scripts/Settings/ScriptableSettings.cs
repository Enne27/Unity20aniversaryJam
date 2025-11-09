using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Game Settings/New Game Settings")]
public class Settings : ScriptableObject
{
    [Header("Gameplay Settings")]
    [SerializeField, Range(0.01f,1)] public float mouseCameraSensitivity = 0.1f;

    [Header("Audio Settings")]
    [SerializeField, Range(0,1)] public float masterVolume = 1; 
    [SerializeField, Range(0,1)] public float musicVolume = 1; 
    [SerializeField, Range(0,1)] public float sfxVolume = 1;
    [SerializeField] public bool muteAll = false; 

}
