using UnityEngine;

public class paperfeedcontroller : MonoBehaviour
{
    public Transform frontpaper;
    public Transform backpaper;

    public float linefeed = 0.02f;

    public int currentline = 0;

    Vector3 frontstartscale;
    Vector3 backstartscale;

    Vector3 frontstartpos;
    Vector3 backstartpos;

    void Start()
    {
        if (frontpaper != null)
        {
            frontstartscale = frontpaper.localScale;
            frontstartpos = frontpaper.localPosition;
        }

        if (backpaper != null)
        {
            backstartscale = backpaper.localScale;
            backstartpos = backpaper.localPosition;
        }

        applypaper();
    }

    public void newline()
    {
        currentline++;
        applypaper();
    }

    void applypaper()
    {
        float delta = currentline * linefeed;

        if (frontpaper != null)
        {
            Vector3 s = frontstartscale;
            s.y += delta;
            frontpaper.localScale = s;

            Vector3 p = frontstartpos;
            p.y += delta * 0.5f;
            frontpaper.localPosition = p;
        }

        if (backpaper != null)
        {
            Vector3 s = backstartscale;
            s.y -= delta;
            backpaper.localScale = s;

            Vector3 p = backstartpos;
            p.y -= delta * 0.5f;
            backpaper.localPosition = p;
        }
    }
}