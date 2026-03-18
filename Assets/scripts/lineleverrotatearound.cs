using UnityEngine;
using System.Collections;

public class lineleverrotate : MonoBehaviour
{
    public Transform pivot;

    public float angle = 25f;
    public float speed = 200f;

    bool moving = false;

    public void pulllever()
    {
        if (moving) return;
        if (pivot == null) return;

        StartCoroutine(rotate());
    }

    IEnumerator rotate()
    {
        moving = true;

        float moved = 0f;

        while (moved < angle)
        {
            float step = speed * Time.deltaTime;
            if (moved + step > angle) step = angle - moved;

            transform.RotateAround(pivot.position, pivot.up, step);

            moved += step;
            yield return null;
        }

        while (moved > 0f)
        {
            float step = speed * Time.deltaTime;
            if (moved - step < 0f) step = moved;

            transform.RotateAround(pivot.position, pivot.up, -step);

            moved -= step;
            yield return null;
        }

        moving = false;
    }
}