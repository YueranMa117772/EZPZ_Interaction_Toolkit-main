using UnityEngine;

public class glyphboardinput : MonoBehaviour
{
    public glyphuniversecenter universecenter;

    public bool shiftactive = false;

    public void receivebasecharacter(string basecharacter)
    {
        if (universecenter == null) return;

        string finalchar = basecharacter;

        if (shiftactive && basecharacter.Length == 1)
        {
            finalchar = basecharacter.ToUpper();
        }

        universecenter.spawnglyph(finalchar);
    }

    public void shiftdown()
    {
        shiftactive = true;
    }

    public void shiftup()
    {
        shiftactive = false;
    }
}