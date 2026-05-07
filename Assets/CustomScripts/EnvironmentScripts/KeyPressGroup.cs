using UnityEngine;

/// Shared key press settings.
public class KeyPressGroup : MonoBehaviour
{
    public float PressDistance = 0.01f;
    public float PressSpeed = 0.10f;
    public float ReturnSpeed = 0.10f;

    public float GetPressTime()
    {
        return PressDistance / PressSpeed;
    }

    public float GetReturnTime()
    {
        return PressDistance / ReturnSpeed;
    }
}