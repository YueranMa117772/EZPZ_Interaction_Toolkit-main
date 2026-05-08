using System.Collections;
using UnityEngine;

/// Makes the object grow when active.
public class ActiveGrow : MonoBehaviour
{
    public float StartScale = 0.1f;
    public float Duration = 0.3f;

    private Vector3 normalScale;

    private void Awake()
    {
        normalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = normalScale * StartScale;
        StartCoroutine(ScaleUp());
    }

    private IEnumerator ScaleUp()
    {
        float timer = 0f;

        while (timer < Duration)
        {
            timer += Time.deltaTime;
            float t = timer / Duration;
            transform.localScale = Vector3.Lerp(normalScale * StartScale, normalScale, t);
            yield return null;
        }

        transform.localScale = normalScale;
    }
}