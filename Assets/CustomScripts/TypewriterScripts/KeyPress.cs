using UnityEngine;

/// Moves a key when pressed.
public class KeyPress : MonoBehaviour
{
    private KeyPressGroup keyGroup;
    private Vector3 startLocalPos;
    private bool isPressed;

    private void Awake()
    {
        keyGroup = GetComponentInParent<KeyPressGroup>();
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (keyGroup == null) return;

        Vector3 targetPos = isPressed
            ? startLocalPos + Vector3.down * keyGroup.PressDistance
            : startLocalPos;

        float speed = isPressed
            ? keyGroup.PressSpeed
            : keyGroup.ReturnSpeed;

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