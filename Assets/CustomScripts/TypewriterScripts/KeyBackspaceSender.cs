using UnityEngine;

/// Sends backspace to glyph center.
public class KeyBackspaceSender : MonoBehaviour
{
    public GlyphUniverseCenter UniverseCenter;

    public void SendBackspace()
    {
        if (UniverseCenter == null)
        {
            return;
        }

        UniverseCenter.Backspace();
    }
}