using UnityEngine;

public class keypress : MonoBehaviour
{
    public float pressDistance = 0.01f;
    public float pressSpeed = 0.10f;
    public float returnSpeed = 0.10f;

    private Vector3 startLocalPos;
    private Vector3 pressedLocalPos;
    private bool isPressed = false;

    void Start()
    {
        startLocalPos = transform.localPosition;
        pressedLocalPos = startLocalPos + Vector3.down * pressDistance;
    }

    void Update()
    {
        Vector3 targetPos = isPressed ? pressedLocalPos : startLocalPos;
        float speed = isPressed ? pressSpeed : returnSpeed;

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetPos,
            speed * Time.deltaTime
        );
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