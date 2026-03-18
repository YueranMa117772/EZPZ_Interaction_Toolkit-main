using UnityEngine;

public class glyphboardlinefeed : MonoBehaviour
{
    public carriagecontroller carriage;
    public Vector3 linestep = new Vector3(0f, 0.02f, 0.005f);

    public int maxline = 3;

    int line = 0;

    public void feedline()
    {
        if (line >= maxline) return;

        line++;
        updateoffset();
    }

    public void backline()
    {
        if (line <= 0) return;

        line--;
        updateoffset();
    }

    void updateoffset()
    {
        if (carriage == null) return;
        carriage.setlineoffset(linestep * line);
    }
}