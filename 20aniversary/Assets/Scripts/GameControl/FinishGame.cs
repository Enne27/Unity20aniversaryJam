using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public bool canFinish = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canFinish)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ScenesManager.Instance.ChangeScene("CreditsScene");
            }
        }
    }
}
