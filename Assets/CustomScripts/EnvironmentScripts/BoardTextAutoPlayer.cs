using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// Plays board text through key bridge.
public class BoardTextAutoPlayer : MonoBehaviour
{
    public BoardKeyboardInput Board;
    public CharacterToInteractableKeyBridge LeftKeyBridge;
    public CharacterToInteractableKeyBridge RightKeyBridge;

    public char ShiftTriggerCharacter = '^';
    public char ReturnCharacterAfterNewLine = '~';

    public UnityEvent OnPlaybackComplete;

    public bool IsPlaying { get; private set; }

    public void PlayBoardText()
    {
        if (IsPlaying) return;
        if (Board == null) return;
        if (LeftKeyBridge == null) return;
        if (RightKeyBridge == null) return;

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
            bool needsShift = char.IsLetter(c) && char.IsUpper(c);

            if (needsShift != shiftIsOn)
            {
                yield return PlayCharacter(ShiftTriggerCharacter);
                shiftIsOn = needsShift;
            }

            yield return PlayCharacter(c);

            if (c == '\n')
            {
                yield return PlayReturnToLineStart(currentLineLength);
                currentLineLength = 0;
            }
            else
            {
                currentLineLength++;
            }
        }

        if (shiftIsOn)
        {
            yield return PlayCharacter(ShiftTriggerCharacter);
        }

        IsPlaying = false;
        OnPlaybackComplete.Invoke();
    }

    private IEnumerator PlayReturnToLineStart(int returnCount)
    {
        for (int i = 0; i < returnCount; i++)
        {
            yield return PlayCharacter(ReturnCharacterAfterNewLine);
        }
    }

    private IEnumerator PlayCharacter(char c)
    {
        yield return LeftKeyBridge.PlayCharacter(c);
        yield return RightKeyBridge.PlayCharacter(c);
    }
}