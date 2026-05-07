using UnityEngine;

/// Sends typed characters to the glyph center.
public class GlyphBoardInput : MonoBehaviour
{
    public GlyphUniverseCenter UniverseCenter;
    public bool ShiftActive;

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

    public void ShiftDown()
    {
        ShiftActive = true;
    }

    public void ShiftUp()
    {
        ShiftActive = false;
    }
}