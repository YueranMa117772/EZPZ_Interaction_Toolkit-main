using UnityEngine;
using System.Collections;

/// Rotates the line lever.
public class LineLeverRotate : MonoBehaviour
{
    public Transform Pivot;
    public float RotateAngle = 25f;
    public float RotateSpeed = 200f;

    private bool isMoving;

    public void PullLever()
    {
        if (isMoving)
        {
            return;
        }

        if (Pivot == null)
        {
            return;
        }

        StartCoroutine(RotateLever());
    }

    private IEnumerator RotateLever()
    {
        isMoving = true;

        float currentAngle = 0f;

        while (currentAngle < RotateAngle)
        {
            float step = RotateSpeed * Time.deltaTime;

            if (currentAngle + step > RotateAngle)
            {
                step = RotateAngle - currentAngle;
            }

            transform.RotateAround(Pivot.position, Pivot.up, step);

            currentAngle += step;
            yield return null;
        }

        while (currentAngle > 0f)
        {
            float step = RotateSpeed * Time.deltaTime;

            if (currentAngle - step < 0f)
            {
                step = currentAngle;
            }

            transform.RotateAround(Pivot.position, Pivot.up, -step);

            currentAngle -= step;
            yield return null;
        }

        isMoving = false;
    }
}