using UnityEngine;
using UnityEngine.SceneManagement; // Required for switching scenes

public class SceneChanger : MonoBehaviour
{
    public void OpenGameScene()
    {
        // Replace "GameSceneName" with the actual name of your scene
        SceneManager.LoadScene("GameScene");
    }
}