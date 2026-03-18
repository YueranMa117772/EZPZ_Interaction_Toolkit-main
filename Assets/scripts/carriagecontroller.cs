using UnityEngine;

public class carriagecontroller : MonoBehaviour
{
    public float movespeed = 2f;

    Vector3 startpos;
    Vector3 targetpos;

    void Start()
    {
        startpos = transform.localPosition;
        targetpos = startpos;
    }

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetpos,
            movespeed * Time.deltaTime
        );
    }

    public void setoffset(float offset)
    {
        targetpos = startpos;
        targetpos.x += offset;
    }
}