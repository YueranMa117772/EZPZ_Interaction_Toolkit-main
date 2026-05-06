using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Shift Action")]
    [Tooltip("Character used only to trigger the Shift key. Bind this character to the Shift key collider in CharacterToInteractableKeyBridge.")]
    public char ShiftTriggerCharacter = '^';

    [Tooltip("Whether uppercase letters should automatically toggle Shift.")]
    public bool UseAutoShift = true;

    [Header("New Line Return Action")]
    [Tooltip("Character played after newline to return the typewriter carriage. Bind this character to the Backspace / return key collider in CharacterToInteractableKeyBridge.")]
    public char ReturnCharacterAfterNewLine = '~';

    [Tooltip("Whether newline should automatically play return characters based on the current line length.")]
    public bool ReturnToLineStartAfterNewLine = true;

    [Header("Events")]
    [Tooltip("Called when the full board text playback is complete.")]
    public UnityEvent OnPlaybackComplete;

    [Header("Testing")]
    [Tooltip("Whether to automatically play the board text when the game starts.")]
    public bool AutoPlayOnStart = false;

    [Header("State")]
    [Tooltip("True while the board text is currently being played.")]
    public bool IsPlaying = false;

    private void Start()
    {
        if (AutoPlayOnStart)
        {
            PlayBoardText();
        }
    }

    /// <summary>
    /// Starts playing the current board text from the beginning.
    /// Call this from NMAWalkModified.OnStandAtTypewriter or another external event.
    /// </summary>
    public void PlayBoardText()
    {
        if (IsPlaying) return;

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
        IsPlaying = true;

        int currentLineLength = 0;
        bool shiftIsOn = false;

        foreach (char c in text)
        {
            bool needsShift = UseAutoShift && char.IsLetter(c) && char.IsUpper(c);

            if (needsShift != shiftIsOn)
            {
                yield return KeyBridge.PlayCharacter(ShiftTriggerCharacter);
                shiftIsOn = needsShift;
            }

            yield return KeyBridge.PlayCharacter(c);

            if (c == '\n')
            {
                if (ReturnToLineStartAfterNewLine)
                {
                    yield return PlayReturnToLineStart(currentLineLength);
                }

                currentLineLength = 0;
            }
            else
            {
                currentLineLength++;
            }
        }

        if (shiftIsOn)
        {
            yield return KeyBridge.PlayCharacter(ShiftTriggerCharacter);
        }

        IsPlaying = false;
        OnPlaybackComplete.Invoke();
    }

    private IEnumerator PlayReturnToLineStart(int returnCount)
    {
        for (int i = 0; i < returnCount; i++)
        {
            yield return KeyBridge.PlayCharacter(ReturnCharacterAfterNewLine);
        }
    }
}