using UnityEngine;

/// Toggles Shift on the board.
public class ShiftLockToggle : MonoBehaviour
{
    public GlyphBoardInput Board;

    private bool ShiftOn;

    public void ToggleShift()
    {
        ShiftOn = !ShiftOn;

        if (Board != null)
        {
            Board.ShiftActive = ShiftOn;
        }
    }
}