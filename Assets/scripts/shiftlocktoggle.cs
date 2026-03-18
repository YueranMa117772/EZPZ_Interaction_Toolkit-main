using UnityEngine;

public class shiftlocktoggle : MonoBehaviour
{
    public glyphboardinput board;

    bool shifton = false;

    public void toggleshift()
    {
        shifton = !shifton;

        if (board != null)
            board.shiftactive = shifton;
    }
}