using UnityEngine;

/// Line lever blink switch.
public class ColourBlinkLineLeverSwitch : MonoBehaviour
{
    public ColourBlinkLineLever BlinkScript;
    public bool IsOn;

    private void Start()
    {
        SetOn(IsOn);
    }

    public void Toggle()
    {
        SetOn(!IsOn);
    }

    public void SetOn(bool on)
    {
        IsOn = on;
        BlinkScript.enabled = IsOn;

        if (!IsOn)
            TurnEmissionOff();
    }

    private void TurnEmissionOff()
    {
        BlinkScript.MyMaterial.SetColor("_EmissionColor", Color.black);
        BlinkScript.MyMaterial.color = BlinkScript.StartColour;
    }
}