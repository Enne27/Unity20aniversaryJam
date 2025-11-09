using UnityEngine;

public class GoToGameScene : MonoBehaviour
{
    public void ChangeToGameScene()
    {
        ScenesManager.Instance.ChangeScene("GameScene");
    }
}
