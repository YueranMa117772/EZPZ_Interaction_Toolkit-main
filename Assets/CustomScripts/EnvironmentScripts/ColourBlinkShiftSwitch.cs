using UnityEngine;

/// Shift blink switch.
public class ColourBlinkShiftSwitch : MonoBehaviour
{
    public ColourBlinkShift BlinkScript;
    public bool IsOn;
    public Color OffColour;

    private void Start()
    {
        BlinkScript.enabled = IsOn;

        if (!IsOn)
            TurnEmissionOff();
    }

    public void Toggle()
    {
        IsOn = !IsOn;
        BlinkScript.enabled = IsOn;

        if (!IsOn)
            TurnEmissionOff();
    }

    private void TurnEmissionOff()
    {
        BlinkScript.MyMaterial.SetColor("_EmissionColor", Color.black);
        BlinkScript.MyMaterial.color = OffColour;
    }
}