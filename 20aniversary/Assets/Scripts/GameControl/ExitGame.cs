using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void ExitTheGame()
    {
        ScenesManager.Instance.ExitGame();
    }
}
