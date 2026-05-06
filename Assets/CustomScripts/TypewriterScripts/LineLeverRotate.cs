using UnityEngine;
using System.Collections;

/// <summary>
/// Controls a lever that rotates around a pivot when pulled, then returns to its original position.
/// </summary>
public class LineLeverRotate : MonoBehaviour
{
    [Tooltip("The pivot point that the lever rotates around.")]
    public Transform Pivot;

    [Tooltip("How far the lever rotates when pulled.")]
    public float RotateAngle = 25f;

    [Tooltip("How fast the lever rotates.")]
    public float RotateSpeed = 200f;

    private bool isMoving;

    /// <summary>
    /// Starts the lever pull and return movement.
    /// </summary>
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