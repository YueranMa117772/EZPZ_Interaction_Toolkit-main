using UnityEngine;

/// Draws a line between two pivots.
[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{
    public Transform StartPivot;
    public Transform EndPivot;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void LateUpdate()
    {
        if (StartPivot == null || EndPivot == null)
        {
            return;
        }

        lineRenderer.SetPosition(0, StartPivot.position);
        lineRenderer.SetPosition(1, EndPivot.position);
    }
}