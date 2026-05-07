using UnityEngine;

/// Controls carriage and glyph board movement.
public class CarriageController : MonoBehaviour
{
    public float MoveSpeed = 2f;
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

    public void SetOffset(float offset)
    {
        currentXOffset = offset;
        UpdateTargets();
    }

    public void SetLineOffset(Vector3 offset)
    {
        currentLineOffset = offset;
        UpdateTargets();
    }

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