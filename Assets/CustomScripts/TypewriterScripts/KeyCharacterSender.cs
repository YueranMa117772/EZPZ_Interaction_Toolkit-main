using UnityEngine;

/// <summary>
/// Sends a predefined base character to the assigned board when triggered.
/// </summary>
public class KeyCharacterSender : MonoBehaviour
{
    [Tooltip("Base character that will be sent to the board.")]
    public string BaseCharacter = "a";

    [Tooltip("Board script that receives the base character.")]
    public GlyphBoardInput Board;

    /// <summary>
    /// Sends the base character to the assigned board.
    /// </summary>
    public void SendCharacter()
    {
        if (Board == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(BaseCharacter))
        {
            return;
        }

        Board.ReceiveBaseCharacter(BaseCharacter);
    }
}