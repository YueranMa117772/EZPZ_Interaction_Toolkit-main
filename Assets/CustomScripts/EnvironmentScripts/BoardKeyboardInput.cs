using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Receives physical keyboard text input during Play Mode
/// and displays it on the board TMP text.
/// </summary>
public class BoardKeyboardInput : MonoBehaviour
{
    [Header("Display")]
    [Tooltip("TMP text component used to display the current board text.")]
    public TMP_Text DisplayText;

    [Header("Input")]
    [Tooltip("Maximum number of characters allowed on the board.")]
    public int MaxCharacters = 60;

    [Tooltip("Maximum number of characters allowed on one line before Enter is required.")]
    public int MaxCharactersPerLine = 20;

    [Tooltip("Whether keyboard typing is currently allowed.")]
    public bool CanType = true;

    [Tooltip("Extra allowed symbol characters for the retro typewriter input.")]
    [SerializeField] private string allowedSymbols = ",.\"/!:";

    [Header("Testing")]
    [Tooltip("Text used when the game starts. Leave empty to start with an empty board.")]
    [TextArea]
    public string StartingText = "";

    private string currentText = "";

    private void Awake()
    {
        currentText = StartingText;

        if (currentText == null)
        {
            currentText = "";
        }

        if (currentText.Length > MaxCharacters)
        {
            currentText = currentText.Substring(0, MaxCharacters);
        }

        UpdateDisplay();
    }

    private void OnEnable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += AddCharacter;
        }
    }

    private void OnDisable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= AddCharacter;
        }
    }

    private void Update()
    {
        if (!CanType) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            DeleteLastCharacter();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            AddCharacter('\n');
        }
    }

    private void AddCharacter(char c)
    {
        if (!CanType) return;
        if (currentText.Length >= MaxCharacters) return;

        if (c == '\r') return;
        if (c == '\b') return;

        if (c != '\n' && !IsAllowedCharacter(c)) return;

        if (c != '\n' && GetCurrentLineLength() >= MaxCharactersPerLine) return;

        currentText += c;
        UpdateDisplay();
    }

    /// <summary>
    /// Checks whether the character is allowed for the retro typewriter input.
    /// </summary>
    private bool IsAllowedCharacter(char c)
    {
        if (char.IsLetterOrDigit(c)) return true;

        return allowedSymbols.Contains(c);
    }

    /// <summary>
    /// Returns the number of characters on the current line.
    /// </summary>
    private int GetCurrentLineLength()
    {
        int lastNewLineIndex = currentText.LastIndexOf('\n');

        if (lastNewLineIndex < 0)
        {
            return currentText.Length;
        }

        return currentText.Length - lastNewLineIndex - 1;
    }

    private void DeleteLastCharacter()
    {
        if (currentText.Length <= 0) return;

        currentText = currentText.Substring(0, currentText.Length - 1);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (DisplayText == null) return;

        DisplayText.text = currentText + "|";
    }

    /// <summary>
    /// Returns the current text stored on the board.
    /// </summary>
    public string GetText()
    {
        return currentText;
    }

    /// <summary>
    /// Clears the current board text and updates the TMP display.
    /// </summary>
    public void ClearText()
    {
        currentText = "";
        UpdateDisplay();
    }

    /// <summary>
    /// Sets whether keyboard typing is currently allowed.
    /// </summary>
    public void SetCanType(bool canType)
    {
        CanType = canType;
    }
}