using UnityEditor;
using UnityEngine;

public class LineRenameTool : EditorWindow
{
    [MenuItem("Tools/Rename Lines To Line Format")]
    private static void RenameLines()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            RenameObjectAndChildren(obj);
        }
    }

    private static void RenameObjectAndChildren(GameObject obj)
    {
        string oldName = obj.name;

        if (oldName.StartsWith("line"))
        {
            string suffix = oldName.Substring("line".Length);

            if (!string.IsNullOrEmpty(suffix))
            {
                string newSuffix = FormatSuffix(suffix);

                Undo.RecordObject(obj, "Rename Line Object");
                obj.name = "Line_" + newSuffix;
                EditorUtility.SetDirty(obj);
            }
        }

        foreach (Transform child in obj.transform)
        {
            RenameObjectAndChildren(child.gameObject);
        }
    }

    private static string FormatSuffix(string suffix)
    {
        if (string.IsNullOrEmpty(suffix))
            return suffix;

        if (suffix.Length == 1)
            return suffix.ToUpper();

        return char.ToUpper(suffix[0]) + suffix.Substring(1).ToLower();
    }
}