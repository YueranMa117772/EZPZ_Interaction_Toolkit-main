using UnityEngine;

/// <summary>
/// Controls the carriage movement and the linked glyph board movement
/// by applying horizontal offset and line offset targets.
/// </summary>
public class CarriageController : MonoBehaviour
{
    [Tooltip("Movement speed used when moving the carriage and glyph board toward their targets.")]
    public float MoveSpeed = 2f;

    [Tooltip("Glyph board root that moves together with the carriage.")]
    public Transform GlyphBoardRoot;

    private Vector3 startLocalPosition;
    private Vector3 targetLocalPosition;

    private Vector3 glyphBoardStartLocalPosition;
    private Vector3 glyphBoardTargetLocalPosition;

    private float currentXOffset;
    private Vector3 currentLineOffset;

    private void Start()
    {
        startLocalPosition = transform.localPosition;
        targetLocalPosition = startLocalPosition;

        if (GlyphBoardRoot != null)
        {
            glyphBoardStartLocalPosition = GlyphBoardRoot.localPosition;
            glyphBoardTargetLocalPosition = glyphBoardStartLocalPosition;
        }

        UpdateTargets();
    }

    private void Update()
    {
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetLocalPosition,
            MoveSpeed * Time.deltaTime
        );

        if (GlyphBoardRoot != null)
        {
            GlyphBoardRoot.localPosition = Vector3.MoveTowards(
                GlyphBoardRoot.localPosition,
                glyphBoardTargetLocalPosition,
                MoveSpeed * Time.deltaTime
            );
        }
    }

    /// <summary>
    /// Sets the current horizontal offset for the carriage system.
    /// </summary>
    public void SetOffset(float offset)
    {
        currentXOffset = offset;
        UpdateTargets();
    }

    /// <summary>
    /// Sets the current line offset applied to the glyph board.
    /// </summary>
    public void SetLineOffset(Vector3 offset)
    {
        currentLineOffset = offset;
        UpdateTargets();
    }

    /// <summary>
    /// Rebuilds the target positions for the carriage and glyph board.
    /// </summary>
    private void UpdateTargets()
    {
        targetLocalPosition = startLocalPosition;
        targetLocalPosition.x += currentXOffset;

        if (GlyphBoardRoot != null)
        {
            glyphBoardTargetLocalPosition = glyphBoardStartLocalPosition;
            glyphBoardTargetLocalPosition.x += currentXOffset;
            glyphBoardTargetLocalPosition += currentLineOffset;
        }
    }
}