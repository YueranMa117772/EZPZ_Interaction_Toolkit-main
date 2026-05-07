using UnityEngine;

/// Blinks line lever light.
public class ColourBlinkLineLever : MonoBehaviour
{
    [Header("Animation Timing Settings")]
    public float BlinkClock;
    public float ClockSpeed = 1;
    public float MaxClock = 1;
    public AnimationCurve BlinkCurve;

    [Header("Colour Settings")]
    public Color CurrentColour;
    public Color StartColour;
    public Color EndColour;

    [Header("System Settings")]
    public float LerpValue;
    public Material MyMaterial;

    void Awake()
    {
        MyMaterial = GetComponent<MeshRenderer>().material;
        CurrentColour = MyMaterial.color;
    }

    void Update()
    {
        if (BlinkClock > MaxClock)
            BlinkClock = 0;

        LerpValue = BlinkCurve.Evaluate(BlinkClock);
        CurrentColour = Color.Lerp(StartColour, EndColour, LerpValue);

        MyMaterial.SetColor("_EmissionColor", CurrentColour);
        MyMaterial.color = CurrentColour;

        BlinkClock += Time.deltaTime * ClockSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        ClockSpeed = newSpeed;
    }
}