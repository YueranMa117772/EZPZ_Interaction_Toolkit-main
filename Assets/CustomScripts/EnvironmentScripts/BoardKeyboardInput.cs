using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// Board input for the visible typing board.
public class BoardKeyboardInput : MonoBehaviour
{
    [Header("Display")]
    public TMP_Text DisplayText;

    [Header("Input")]
    public int MaxCharactersPerLine = 20;
    public int MaxLines = 3;
    public bool CanType = true;

    [SerializeField] private string allowedSymbols = ",.\"/!:";

    [Header("Testing")]
    [TextArea]
    public string StartingText = "";

    private string currentText = "";

    private void Awake()
    {
        currentText = StartingText;
        UpdateDisplay();
    }

    private void OnEnable()
    {
        if (Keyboard.current != null)
            Keyboard.current.onTextInput += AddCharacter;
    }

    private void OnDisable()
    {
        if (Keyboard.current != null)
            Keyboard.current.onTextInput -= AddCharacter;
    }

    private void Update()
    {
        if (!CanType || Keyboard.current == null) return;

        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            DeleteLastCharacter();

        if (Keyboard.current.enterKey.wasPressedThisFrame)
            AddCharacter('\n');
    }

    private void AddCharacter(char c)
    {
        if (!CanType || c == '\r') return;

        if (c == '\n')
        {
            if (LineCount() >= MaxLines) return;

            currentText += "\n";
            UpdateDisplay();
            return;
        }

        if (!IsAllowedCharacter(c)) return;
        if (CurrentLineLength() >= MaxCharactersPerLine) return;

        currentText += c;
        UpdateDisplay();
    }

    private bool IsAllowedCharacter(char c)
    {
        return char.IsLetterOrDigit(c) || allowedSymbols.Contains(c);
    }

    private int CurrentLineLength()
    {
        int lastLineBreak = currentText.LastIndexOf('\n');

        if (lastLineBreak < 0)
            return currentText.Length;

        return currentText.Length - lastLineBreak - 1;
    }

    private int LineCount()
    {
        int count = 1;

        foreach (char c in currentText)
        {
            if (c == '\n')
                count++;
        }

        return count;
    }

    private void DeleteLastCharacter()
    {
        if (currentText.Length == 0) return;

        currentText = currentText.Substring(0, currentText.Length - 1);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        DisplayText.text = currentText + "|";
    }

    public string GetText()
    {
        return currentText;
    }

    public void SetCanType(bool canType)
    {
        CanType = canType;
    }
}