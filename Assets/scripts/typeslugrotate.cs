using UnityEngine;

public class typeslugrotate : MonoBehaviour
{
    public Transform pivot;

    public float rotateAngle = 30f;
    public float rotateSpeed = 200f;
    public float returnSpeed = 200f;

    float currentAngle = 0f;
    bool isPressed = false;

    Vector3 startPosition;
    Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (pivot == null) return;

        float targetAngle = isPressed ? rotateAngle : 0f;
        float speed = isPressed ? rotateSpeed : returnSpeed;

        currentAngle = Mathf.MoveTowards(
            currentAngle,
            targetAngle,
            speed * Time.deltaTime
        );

        transform.position = startPosition;
        transform.rotation = startRotation;

        transform.RotateAround(pivot.position, pivot.up, -currentAngle);
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