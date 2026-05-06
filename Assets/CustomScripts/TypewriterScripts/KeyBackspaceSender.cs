using UnityEngine;

/// <summary>
/// Sends backspace input from a key object to the glyph universe center.
/// </summary>
public class KeyBackspaceSender : MonoBehaviour
{
    [Tooltip("Glyph universe center that receives the backspace command.")]
    public GlyphUniverseCenter UniverseCenter;

    /// <summary>
    /// Triggers the backspace action if a glyph universe center is assigned.
    /// </summary>
    public void SendBackspace()
    {
        if (UniverseCenter == null)
        {
            return;
        }

        UniverseCenter.Backspace();
    }
}