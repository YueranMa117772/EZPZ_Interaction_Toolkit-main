using UnityEngine;

/// <summary>
/// Rotates the type slug around a specified pivot point when pressed
/// and returns it to its starting position when released.
/// </summary>
public class TypeSlugRotate : MonoBehaviour
{
    [Tooltip("Transform used as the pivot point and rotation axis reference.")]
    public Transform Pivot;

    [Tooltip("Target rotation angle in degrees when the type slug is pressed.")]
    public float RotateAngle = 30f;

    [Tooltip("Rotation speed in degrees per second while moving toward the pressed angle.")]
    public float RotateSpeed = 200f;

    [Tooltip("Rotation speed in degrees per second while returning to the resting state.")]
    public float ReturnSpeed = 200f;

    private float currentAngle;
    private bool isPressed;

    private Vector3 startPosition;
    private Quaternion startRotation;

    /// <summary>
    /// Stores the initial world position and rotation of the type slug.
    /// </summary>
    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    /// <summary>
    /// Updates the current rotation each frame and applies the movement around the pivot.
    /// </summary>
    private void Update()
    {
        if (Pivot == null)
        {
            return;
        }

        float targetAngle = isPressed ? RotateAngle : 0f;
        float speed = isPressed ? RotateSpeed : ReturnSpeed;

        currentAngle = Mathf.MoveTowards(
            currentAngle,
            targetAngle,
            speed * Time.deltaTime
        );

        transform.position = startPosition;
        transform.rotation = startRotation;

        transform.RotateAround(Pivot.position, Pivot.up, -currentAngle);
    }

    /// <summary>
    /// Sets the type slug to the pressed state.
    /// </summary>
    public void PressKey()
    {
        isPressed = true;
    }

    /// <summary>
    /// Sets the type slug to the released state.
    /// </summary>
    public void ReleaseKey()
    {
        isPressed = false;
    }
}