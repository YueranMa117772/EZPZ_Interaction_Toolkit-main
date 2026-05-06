using UnityEngine;

/// <summary>
/// Stores shared press settings for all keys in this key group.
/// </summary>
public class KeyPressGroup : MonoBehaviour
{
    [Tooltip("How far each key moves downward when pressed.")]
    [Min(0.001f)]
    public float PressDistance = 0.01f;

    [Tooltip("How fast each key moves downward when pressed.")]
    [Min(0.001f)]
    public float PressSpeed = 0.10f;

    [Tooltip("How fast each key returns to its starting position.")]
    [Min(0.001f)]
    public float ReturnSpeed = 0.10f;

    /// <summary>
    /// Returns how long a key takes to move from its start position to its pressed position.
    /// </summary>
    public float GetPressTime()
    {
        return PressDistance / PressSpeed;
    }

    /// <summary>
    /// Returns how long a key takes to move from its pressed position back to its start position.
    /// </summary>
    public float GetReturnTime()
    {
        return PressDistance / ReturnSpeed;
    }
}