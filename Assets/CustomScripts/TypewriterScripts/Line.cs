using UnityEngine;

/// <summary>
/// Updates a LineRenderer so it connects between a start pivot and an end pivot.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{
    [Tooltip("Start point of the rendered line.")]
    public Transform StartPivot;

    [Tooltip("End point of the rendered line.")]
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