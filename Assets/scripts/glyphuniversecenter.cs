using UnityEngine;
using TMPro;

public class glyphuniversecenter : MonoBehaviour
{
    public Transform universecenter;
    public Transform glyphboardroot;
    public GameObject glyphprefab;

    public Transform shifttarget;
    public bool followshift = true;

    public BoxCollider glyphpaperarea;
    public carriagecontroller carriage;

    float currentoffset = 0f;

    public void spawnglyph(string character)
    {
        if (universecenter == null) return;
        if (glyphboardroot == null) return;
        if (glyphprefab == null) return;

        updateuniversecenter();

        if (!isinsidepaperarea(universecenter.position)) return;

        Quaternion glyphrot =
            universecenter.rotation * Quaternion.Euler(0f, 180f, 0f);

        GameObject g = Instantiate(
            glyphprefab,
            universecenter.position,
            glyphrot,
            glyphboardroot
        );

        TMP_Text t = g.GetComponent<TMP_Text>();
        if (t == null) t = g.GetComponentInChildren<TMP_Text>();

        if (t != null) t.text = character;

        glyphadvance a = glyphprefab.GetComponent<glyphadvance>();
        if (a == null) return;

        currentoffset += a.advance;

        if (carriage != null)
            carriage.setoffset(currentoffset);
    }

    public void backspace()
    {
        if (glyphprefab == null) return;

        glyphadvance a = glyphprefab.GetComponent<glyphadvance>();
        if (a == null) return;

        currentoffset += a.backadvance();

        if (currentoffset < 0f)
            currentoffset = 0f;

        if (carriage != null)
            carriage.setoffset(currentoffset);
    }

    bool isinsidepaperarea(Vector3 worldpos)
    {
        if (glyphpaperarea == null) return true;
        return glyphpaperarea.bounds.Contains(worldpos);
    }

    void updateuniversecenter()
    {
        if (!followshift) return;
        if (shifttarget == null) return;

        universecenter.position = shifttarget.position;
        universecenter.rotation = shifttarget.rotation;
    }
}