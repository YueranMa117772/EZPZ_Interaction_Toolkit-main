using UnityEngine;

/// <summary>
/// Controls the press and return movement of a key object.
/// Uses the nearest parent KeyPressGroup for shared movement settings.
/// </summary>
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

    /// <summary>
    /// Sets the key to the pressed state.
    /// </summary>
    public void PressKey()
    {
        isPressed = true;
    }

    /// <summary>
    /// Sets the key to the released state.
    /// </summary>
    public void ReleaseKey()
    {
        isPressed = false;
    }
}