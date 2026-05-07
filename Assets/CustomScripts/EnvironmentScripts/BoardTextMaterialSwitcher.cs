using TMPro;
using UnityEngine;

/// <summary>
/// Switches the board TMP text between normal and confirmed material presets.
/// </summary>
public class BoardTextMaterialSwitcher : MonoBehaviour
{
    [Header("Text")]
    [Tooltip("The TMP text shown on the board.")]
    public TMP_Text BoardText;

    [Header("Material Presets")]
    [Tooltip("Material used while the text is still editable.")]
    public Material NormalMaterial;

    [Tooltip("Material used after the text has been confirmed.")]
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