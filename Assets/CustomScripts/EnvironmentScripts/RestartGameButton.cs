using UnityEngine;
using UnityEngine.SceneManagement;

/// Reloads the current scene.
public class RestartGameButton : MonoBehaviour
{
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}