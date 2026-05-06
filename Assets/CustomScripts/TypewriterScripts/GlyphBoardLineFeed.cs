using UnityEngine;

/// <summary>
/// Controls line feed movement by changing the carriage line offset.
/// </summary>
public class GlyphBoardLineFeed : MonoBehaviour
{
    [Tooltip("Carriage controller that receives the line offset.")]
    public CarriageController Carriage;

    [Tooltip("Offset added for each new line.")]
    public Vector3 LineStep = new Vector3(0f, 0.02f, 0.005f);

    [Tooltip("Maximum number of lines allowed.")]
    public int MaxLine = 3;

    private int line;

    /// <summary>
    /// Moves forward by one line if the maximum line count has not been reached.
    /// </summary>
    public void FeedLine()
    {
        if (line >= MaxLine)
        {
            return;
        }

        line++;
        UpdateOffset();
    }

    /// <summary>
    /// Moves backward by one line if the current line is above zero.
    /// </summary>
    public void BackLine()
    {
        if (line <= 0)
        {
            return;
        }

        line--;
        UpdateOffset();
    }

    /// <summary>
    /// Updates the carriage line offset based on the current line index.
    /// </summary>
    private void UpdateOffset()
    {
        if (Carriage == null)
        {
            return;
        }

        Carriage.SetLineOffset(LineStep * line);
    }
}