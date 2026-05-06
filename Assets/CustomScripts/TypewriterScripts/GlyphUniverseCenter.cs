using UnityEngine;
using TMPro;

/// <summary>
/// Controls the glyph spawn position, creates glyph objects,
/// and updates carriage offset based on glyph advance values.
/// </summary>
public class GlyphUniverseCenter : MonoBehaviour
{
    [Tooltip("Transform used as the glyph spawn point.")]
    public Transform UniverseCenter;

    [Tooltip("Parent transform that holds all spawned glyph objects.")]
    public Transform GlyphBoardRoot;

    [Tooltip("Prefab used to spawn each glyph.")]
    public GameObject GlyphPrefab;

    [Tooltip("Paper area that limits where glyphs can be spawned.")]
    public BoxCollider GlyphPaperArea;

    [Tooltip("Carriage controller that receives updated offset values.")]
    public CarriageController Carriage;

    private float currentOffset;

    /// <summary>
    /// Spawns a glyph with the given character at the current universe center position.
    /// Updates the carriage offset using the glyph advance value.
    /// </summary>
    public void SpawnGlyph(string character)
    {
        if (UniverseCenter == null)
        {
            return;
        }

        if (GlyphBoardRoot == null)
        {
            return;
        }

        if (GlyphPrefab == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(character))
        {
            return;
        }

        if (!IsInsidePaperArea(UniverseCenter.position))
        {
            return;
        }

        Quaternion glyphRotation =
            UniverseCenter.rotation * Quaternion.Euler(0f, 180f, 0f);

        GameObject glyphObject = Instantiate(
            GlyphPrefab,
            UniverseCenter.position,
            glyphRotation,
            GlyphBoardRoot
        );

        TMP_Text glyphText = glyphObject.GetComponent<TMP_Text>();

        if (glyphText == null)
        {
            glyphText = glyphObject.GetComponentInChildren<TMP_Text>();
        }

        if (glyphText != null)
        {
            glyphText.text = character;
        }

        GlyphAdvance advanceData = GlyphPrefab.GetComponent<GlyphAdvance>();

        if (advanceData == null)
        {
            return;
        }

        currentOffset += advanceData.Advance;

        if (Carriage != null)
        {
            Carriage.SetOffset(currentOffset);
        }
    }

    /// <summary>
    /// Moves the carriage backward using the glyph backspace advance value.
    /// </summary>
    public void Backspace()
    {
        if (GlyphPrefab == null)
        {
            return;
        }

        GlyphAdvance advanceData = GlyphPrefab.GetComponent<GlyphAdvance>();

        if (advanceData == null)
        {
            return;
        }

        currentOffset += advanceData.BackAdvance();

        if (currentOffset < 0f)
        {
            currentOffset = 0f;
        }

        if (Carriage != null)
        {
            Carriage.SetOffset(currentOffset);
        }
    }

    /// <summary>
    /// Checks whether a world position is inside the allowed paper area.
    /// If no paper area is assigned, spawning is allowed everywhere.
    /// </summary>
    private bool IsInsidePaperArea(Vector3 worldPosition)
    {
        if (GlyphPaperArea == null)
        {
            return true;
        }

        return GlyphPaperArea.bounds.Contains(worldPosition);
    }
}