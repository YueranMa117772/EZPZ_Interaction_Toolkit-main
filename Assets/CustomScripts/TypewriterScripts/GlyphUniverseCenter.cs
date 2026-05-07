using UnityEngine;
using TMPro;

/// Spawns glyphs and moves the carriage.
public class GlyphUniverseCenter : MonoBehaviour
{
    public Transform UniverseCenter;
    public Transform GlyphBoardRoot;
    public GameObject GlyphPrefab;
    public BoxCollider GlyphPaperArea;
    public CarriageController Carriage;

    private float currentOffset;

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

    private bool IsInsidePaperArea(Vector3 worldPosition)
    {
        if (GlyphPaperArea == null)
        {
            return true;
        }

        return GlyphPaperArea.bounds.Contains(worldPosition);
    }
}