using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class line : MonoBehaviour
{
    public Transform startPivot;
    public Transform endPivot;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    void LateUpdate()
    {
        if (startPivot == null || endPivot == null) return;

        lineRenderer.SetPosition(0, startPivot.position);
        lineRenderer.SetPosition(1, endPivot.position);
    }
}