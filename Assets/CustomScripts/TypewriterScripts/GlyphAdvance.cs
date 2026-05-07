using UnityEngine;

/// Stores glyph spacing.
public class GlyphAdvance : MonoBehaviour
{
    public float Advance = 0.02f;

    public float BackAdvance()
    {
        return -Advance;
    }
}