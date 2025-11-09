using UnityEngine;

public class GoBackToMainMenu : MonoBehaviour
{
   public void GoBackToMainMenuF()
    {
        ScenesManager.Instance.ChangeScene("MainMenuScene");
    }
}
