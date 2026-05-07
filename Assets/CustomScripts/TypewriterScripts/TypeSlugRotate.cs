using UnityEngine;

/// Rotates the type slug.
public class TypeSlugRotate : MonoBehaviour
{
    public Transform Pivot;
    public float RotateAngle = 30f;
    public float RotateSpeed = 200f;
    public float ReturnSpeed = 200f;

    private float currentAngle;
    private bool isPressed;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

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

    public void PressKey()
    {
        isPressed = true;
    }

    public void ReleaseKey()
    {
        isPressed = false;
    }
}