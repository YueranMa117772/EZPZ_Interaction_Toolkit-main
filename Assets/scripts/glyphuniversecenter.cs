using UnityEngine;
using TMPro;

public class glyphuniversecenter : MonoBehaviour
{
    public Transform universecenter;
    public Transform glyphboardroot;

    public GameObject glyphprefab;

    public Transform shifttarget;
    public bool followshift = true;

    public MonoBehaviour carriagetarget;
    public string carriagemethod = "advanceonestep";

    public void spawnglyph(string character)
    {
        if (universecenter == null) return;
        if (glyphboardroot == null) return;
        if (glyphprefab == null) return;

        updateuniversecenter();

        GameObject g = Instantiate(
            glyphprefab,
            universecenter.position,
            universecenter.rotation,
            glyphboardroot
        );

        TMP_Text t = g.GetComponent<TMP_Text>();
        if (t == null) t = g.GetComponentInChildren<TMP_Text>();

        if (t != null)
        {
            t.text = character;
        }

        triggercarriage();
    }

    void updateuniversecenter()
    {
        if (!followshift) return;
        if (shifttarget == null) return;

        universecenter.position = shifttarget.position;
        universecenter.rotation = shifttarget.rotation;
    }

    void triggercarriage()
    {
        if (carriagetarget == null) return;

        carriagetarget.SendMessage(
            carriagemethod,
            SendMessageOptions.DontRequireReceiver
        );
    }
}