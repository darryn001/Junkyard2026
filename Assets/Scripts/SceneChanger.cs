using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Added this for the focus check

public class SceneChanger : MonoBehaviour
{
    void Update()
    {
        // Detect Shift key press
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            TryReloadScene();
        }
    }

    public void TryReloadScene()
    {
        // SAFETY CHECK: If the player is typing in the Code Panel, STOP.
        if (EventSystem.current != null && EventSystem.current.alreadySelecting)
        {
            Debug.Log("Shift pressed, but player is typing. Ignoring reload.");
            return;
        }

        OpenGameScene();
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}