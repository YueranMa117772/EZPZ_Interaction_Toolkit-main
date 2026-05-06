using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reloads the current scene to restart the game flow.
/// </summary>
public class RestartGameButton : MonoBehaviour
{
    /// <summary>
    /// Restarts the current active scene.
    /// </summary>
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}