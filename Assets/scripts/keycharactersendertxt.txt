using UnityEngine;

public class keycharactersender : MonoBehaviour
{
    public string basecharacter = "a";
    public glyphboardinput board;

    public void sendcharacter()
    {
        if (board == null) return;
        if (string.IsNullOrEmpty(basecharacter)) return;

        board.receivebasecharacter(basecharacter);
    }
}