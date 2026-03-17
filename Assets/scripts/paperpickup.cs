using UnityEngine;

public class paperpickup : MonoBehaviour
{
    public Transform holdpoint;
    public bool held = false;

    void Update()
    {
        if (held && holdpoint != null)
        {
            transform.position = holdpoint.position;
            transform.rotation = holdpoint.rotation;
        }
    }

    void OnMouseDown()
    {
        held = true;
    }

    public void drop()
    {
        held = false;
        gameObject.SetActive(false);
    }
}