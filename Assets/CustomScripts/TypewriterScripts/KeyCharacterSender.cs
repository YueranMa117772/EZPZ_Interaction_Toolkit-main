using UnityEngine;

/// Sends one character to the board.
public class KeyCharacterSender : MonoBehaviour
{
    public string BaseCharacter = "a";
    public GlyphBoardInput Board;

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