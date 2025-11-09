using UnityEngine;

public class GoToSettingsScene : MonoBehaviour
{
    public void ChangeToSettingsScene()
    {
        ScenesManager.Instance.ChangeScene("SettingsScene");
    }
}
