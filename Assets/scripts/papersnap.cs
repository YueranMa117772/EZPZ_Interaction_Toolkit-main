using UnityEngine;

public class papersnap : MonoBehaviour
{
    public GameObject internalpaper;

    void OnTriggerEnter(Collider other)
    {
        paperpickup p = other.GetComponent<paperpickup>();

        if (p != null && p.held)
        {
            internalpaper.SetActive(true);
            p.drop();
        }
    }
}