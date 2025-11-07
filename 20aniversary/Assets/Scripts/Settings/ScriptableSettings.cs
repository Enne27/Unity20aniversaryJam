using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Game Settings/New Game Settings")]
public class ScriptableSettings : ScriptableObject
{
    public enum Quality
    {
        Low,
        Medium,
        High,
        Ultra
    }


}
