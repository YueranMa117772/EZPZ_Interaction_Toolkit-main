using UnityEngine;

/// Switches two sfx objects from all child key presses.
public class ChildKeyPressActive : MonoBehaviour
{
    public GameObject PressedSfx;
    public GameObject ReleasedSfx;

    public GameObject ExcludeObject;

    private void Start()
    {
        InteractableGeneral[] keys = GetComponentsInChildren<InteractableGeneral>();

        foreach (InteractableGeneral key in keys)
        {
            if (ExcludeObject != null && key.transform.IsChildOf(ExcludeObject.transform))
                continue;

            key.onPrimaryInteract.AddListener(Press);
            key.onPrimaryInteractLift.AddListener(Release);
        }
    }

    public void Press()
    {
        PressedSfx.SetActive(true);
        ReleasedSfx.SetActive(false);
    }

    public void Release()
    {
        PressedSfx.SetActive(false);
        ReleasedSfx.SetActive(true);
    }
}