using UnityEngine;

public class FinalArtefact : MonoBehaviour
{
    [Header("End Game UI")]
    public GameObject winPanel; // Drag your WinPanel here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Show the Win Screen
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }

            // 2. Unlock the mouse so they can click buttons (optional)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 3. Pause the game
            Time.timeScale = 0;

            // 4. Remove the artifact
            Destroy(gameObject);
        }
    }
}