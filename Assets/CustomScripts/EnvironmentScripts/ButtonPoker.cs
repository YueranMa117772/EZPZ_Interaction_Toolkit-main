using UnityEngine;

/// <summary>
/// Lets an NPC trigger an InteractableGeneral when this trigger collider touches it.
/// Enter = primary interact.
/// Exit = primary interact lift.
/// </summary>
public class ButtonPoker : MonoBehaviour
{
    [Tooltip("The InteractableGeneral currently being touched by this poker.")]
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