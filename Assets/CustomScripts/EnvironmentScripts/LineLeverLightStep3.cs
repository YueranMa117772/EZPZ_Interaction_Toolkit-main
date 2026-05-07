using UnityEngine;

/// Line lever three light steps.
public class LineLeverLightStep3 : MonoBehaviour
{
    public ColourBlinkLineLeverSwitch Light1;
    public ColourBlinkLineLeverSwitch Light2;
    public ColourBlinkLineLeverSwitch Light3;

    public int Step;

    public void Press()
    {
        Step++;

        if (Step > 3)
            Step = 3;

        Light1.SetOn(Step >= 1);
        Light2.SetOn(Step >= 2);
        Light3.SetOn(Step >= 3);
    }
}