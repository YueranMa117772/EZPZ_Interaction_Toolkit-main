using System.Collections;
using UnityEngine;

/// <summary>
/// Reads text from BoardKeyboardInput and plays it back character by character.
/// This script controls character order only.
/// The actual move, press, and release timing is handled by CharacterToInteractableKeyBridge.
/// </summary>
public class BoardTextAutoPlayer : MonoBehaviour
{
    [Header("Input Source")]
    [Tooltip("Board script that stores the text to be played.")]
    public BoardKeyboardInput Board;

    [Header("Output")]
    [Tooltip("Bridge script that receives each character and plays the full key action.")]
    public CharacterToInteractableKeyBridge KeyBridge;

    [Header("Testing")]
    [Tooltip("Whether to automatically play the board text when the game starts.")]
    public bool AutoPlayOnStart = false;

    private void Start()
    {
        if (AutoPlayOnStart)
        {
            PlayBoardText();
        }
    }

    /// <summary>
    /// Starts playing the current board text from the beginning.
    /// Call this from HoldableMagnetSnapper.onSnap or another external event.
    /// </summary>
    public void PlayBoardText()
    {
        if (Board == null)
        {
            Debug.LogWarning("Board is not assigned.", this);
            return;
        }

        if (KeyBridge == null)
        {
            Debug.LogWarning("KeyBridge is not assigned.", this);
            return;
        }

        string text = Board.GetText();

        if (string.IsNullOrEmpty(text)) return;

        StartCoroutine(PlayTextRoutine(text));
    }

    private IEnumerator PlayTextRoutine(string text)
    {
        foreach (char c in text)
        {
            yield return KeyBridge.PlayCharacter(c);
        }
    }
}