using UnityEngine;

/// NPC poker for InteractableGeneral.
public class ButtonPoker : MonoBehaviour
{
    public InteractableGeneral subject;

    private void OnTriggerEnter(Collider other)
    {
        subject = other.GetComponent<InteractableGeneral>();

        if (subject != null)
        {
            subject.onPrimaryInteract.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableGeneral leavingSubject = other.GetComponent<InteractableGeneral>();

        if (leavingSubject != null && leavingSubject == subject)
        {
            subject.onPrimaryInteractLift.Invoke();
            subject = null;
        }
    }
}