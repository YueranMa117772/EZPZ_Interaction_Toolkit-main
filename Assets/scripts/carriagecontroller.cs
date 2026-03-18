using UnityEngine;

public class carriagecontroller : MonoBehaviour
{
    public float movespeed = 2f;
    public Transform glyphboardroot;

    Vector3 startpos;
    Vector3 targetpos;

    Vector3 glyphstartpos;
    Vector3 glyphtargetpos;

    float currentxoffset = 0f;
    Vector3 currentlyneoffset = Vector3.zero;

    void Start()
    {
        startpos = transform.localPosition;
        targetpos = startpos;

        if (glyphboardroot != null)
        {
            glyphstartpos = glyphboardroot.localPosition;
            glyphtargetpos = glyphstartpos;
        }

        updatetargets();
    }

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetpos,
            movespeed * Time.deltaTime
        );

        if (glyphboardroot != null)
        {
            glyphboardroot.localPosition = Vector3.MoveTowards(
                glyphboardroot.localPosition,
                glyphtargetpos,
                movespeed * Time.deltaTime
            );
        }
    }

    public void setoffset(float offset)
    {
        currentxoffset = offset;
        updatetargets();
    }

    public void setlineoffset(Vector3 offset)
    {
        currentlyneoffset = offset;
        updatetargets();
    }

    void updatetargets()
    {
        targetpos = startpos;
        targetpos.x += currentxoffset;

        if (glyphboardroot != null)
        {
            glyphtargetpos = glyphstartpos;
            glyphtargetpos.x += currentxoffset;
            glyphtargetpos += currentlyneoffset;
        }
    }
}