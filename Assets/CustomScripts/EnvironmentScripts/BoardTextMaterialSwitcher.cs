using TMPro;
using UnityEngine;

/// Switches board text material.
public class BoardTextMaterialSwitcher : MonoBehaviour
{
    public TMP_Text BoardText;
    public Material NormalMaterial;
    public Material ConfirmedMaterial;

    private void Start()
    {
        SetNormalMaterial();
    }

    public void SetNormalMaterial()
    {
        if (BoardText == null || NormalMaterial == null) return;

        BoardText.fontSharedMaterial = NormalMaterial;
    }

    public void SetConfirmedMaterial()
    {
        if (BoardText == null || ConfirmedMaterial == null) return;

        BoardText.fontSharedMaterial = ConfirmedMaterial;
    }
}