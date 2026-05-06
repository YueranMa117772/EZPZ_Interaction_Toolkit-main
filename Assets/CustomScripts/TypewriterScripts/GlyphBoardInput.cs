using UnityEngine;

/// <summary>
/// Receives a base character, applies Shift-based case logic,
/// and sends the final character to the universe center.
/// </summary>
public class GlyphBoardInput : MonoBehaviour
{
    [Tooltip("Universe center that receives the final character and spawns the glyph.")]
    public GlyphUniverseCenter UniverseCenter;

    [Tooltip("Whether Shift is currently active.")]
    public bool ShiftActive;

    /// <summary>
    /// Receives a base character, converts it to uppercase if Shift is active,
    /// and sends the final character to the universe center.
    /// </summary>
    public void ReceiveBaseCharacter(string baseCharacter)
    {
        if (UniverseCenter == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(baseCharacter))
        {
            return;
        }

        string finalCharacter = baseCharacter;

        if (ShiftActive && baseCharacter.Length == 1)
        {
            finalCharacter = baseCharacter.ToUpper();
        }

        UniverseCenter.SpawnGlyph(finalCharacter);
    }

    /// <summary>
    /// Activates Shift state.
    /// </summary>
    public void ShiftDown()
    {
        ShiftActive = true;
    }

    /// <summary>
    /// Deactivates Shift state.
    /// </summary>
    public void ShiftUp()
    {
        ShiftActive = false;
    }
}