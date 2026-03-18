using UnityEngine;

public class keybackspacesender : MonoBehaviour
{
    public glyphuniversecenter universecenter;

    public void sendbackspace()
    {
        if (universecenter == null) return;
        universecenter.backspace();
    }
}