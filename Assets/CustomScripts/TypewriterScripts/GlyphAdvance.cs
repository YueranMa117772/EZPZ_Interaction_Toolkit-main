using UnityEngine;

/// <summary>
/// Stores the horizontal advance distance used for glyph spacing.
/// </summary>
public class GlyphAdvance : MonoBehaviour
{
    [Tooltip("Horizontal distance used for one glyph advance step.")]
    public float Advance = 0.02f;

    /// <summary>
    /// Returns the negative advance distance for backward glyph movement.
    /// </summary>
    public float BackAdvance()
    {
        return -Advance;
    }
}