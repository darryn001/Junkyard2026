using UnityEngine;
using UnityEngine.SceneManagement; // Required for restarting the game

public class ButtonManager : MonoBehaviour
{
    // Function to restart the current level
    public void RestartGame()
    {
        // Unpause the game time before restarting
        Time.timeScale = 1;
        // Reloads the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Function to close the application
    public void QuitGame()
    {
        Debug.Log("Game is quitting..."); // Only visible in the editor
        Application.Quit(); // Closes the actual built game (.exe)
    }
}