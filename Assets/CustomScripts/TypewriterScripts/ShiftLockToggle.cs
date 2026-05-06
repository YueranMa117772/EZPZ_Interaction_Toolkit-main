using UnityEngine;

/// <summary>
/// Toggles the Shift state on the assigned glyph board input.
/// </summary>
public class ShiftLockToggle : MonoBehaviour
{
    [Tooltip("Board input that receives the toggled Shift state.")]
    public GlyphBoardInput Board;

    private bool ShiftOn;

    /// <summary>
    /// Toggles Shift on or off and applies the state to the board input.
    /// </summary>
    public void ToggleShift()
    {
        ShiftOn = !ShiftOn;

        if (Board != null)
        {
            Board.ShiftActive = ShiftOn;
        }
    }
}