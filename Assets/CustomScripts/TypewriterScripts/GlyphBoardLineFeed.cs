using UnityEngine;

/// Controls glyph board line feed.
public class GlyphBoardLineFeed : MonoBehaviour
{
    public CarriageController Carriage;
    public Vector3 LineStep = new Vector3(0f, 0.02f, 0.005f);
    public int MaxLine = 3;

    private int line;

    public void FeedLine()
    {
        if (line >= MaxLine)
        {
            return;
        }

        line++;
        UpdateOffset();
    }

    public void BackLine()
    {
        if (line <= 0)
        {
            return;
        }

        line--;
        UpdateOffset();
    }

    private void UpdateOffset()
    {
        if (Carriage == null)
        {
            return;
        }

        Carriage.SetLineOffset(LineStep * line);
    }
}