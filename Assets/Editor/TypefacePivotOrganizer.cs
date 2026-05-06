using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TypefacePivotOrganizer : EditorWindow
{
    [MenuItem("Tools/Organize Typeface And Pivots")]
    private static void OrganizeTypefaceAndPivots()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            Debug.LogWarning("Please select the typeface/pivot objects in the Hierarchy first.");
            return;
        }

        HashSet<GameObject> allObjects = new HashSet<GameObject>();

        foreach (GameObject obj in selectedObjects)
        {
            CollectRecursively(obj.transform, allObjects);
        }

        Dictionary<string, GameObject> typefaceMap = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> pivotMap = new Dictionary<string, GameObject>();

        // 第一步：统一重命名，并记录对应编号/字母
        foreach (GameObject obj in allObjects)
        {
            string oldName = obj.name;

            if (StartsWithIgnoreCase(oldName, "typeface"))
            {
                string suffix = ExtractSuffix(oldName, "typeface");
                string formattedSuffix = FormatSuffix(suffix);
                string newName = "Typeface_" + formattedSuffix;

                Undo.RecordObject(obj, "Rename Typeface");
                obj.name = newName;
                EditorUtility.SetDirty(obj);

                typefaceMap[formattedSuffix] = obj;
            }
            else if (StartsWithIgnoreCase(oldName, "pivot"))
            {
                string suffix = ExtractSuffix(oldName, "pivot");
                string formattedSuffix = FormatSuffix(suffix);
                string newName = "Pivot_" + formattedSuffix;

                Undo.RecordObject(obj, "Rename Pivot");
                obj.name = newName;
                EditorUtility.SetDirty(obj);

                pivotMap[formattedSuffix] = obj;
            }
        }

        // 第二步：把对应 Pivot 挂到对应 Typeface 下面
        foreach (var pair in pivotMap)
        {
            string suffix = pair.Key;
            GameObject pivotObj = pair.Value;

            if (typefaceMap.TryGetValue(suffix, out GameObject typefaceObj))
            {
                if (pivotObj.transform.parent != typefaceObj.transform)
                {
                    Undo.SetTransformParent(pivotObj.transform, typefaceObj.transform, "Parent Pivot To Typeface");
                    pivotObj.transform.SetParent(typefaceObj.transform, true);
                    EditorUtility.SetDirty(pivotObj);
                    EditorUtility.SetDirty(typefaceObj);
                }
            }
            else
            {
                Debug.LogWarning($"No matching Typeface found for Pivot_{suffix}");
            }
        }

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("Finished organizing Typeface and Pivot objects.");
    }

    private static void CollectRecursively(Transform current, HashSet<GameObject> results)
    {
        if (!results.Contains(current.gameObject))
        {
            results.Add(current.gameObject);
        }

        foreach (Transform child in current)
        {
            CollectRecursively(child, results);
        }
    }

    private static bool StartsWithIgnoreCase(string source, string prefix)
    {
        return source.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase);
    }

    private static string ExtractSuffix(string objectName, string prefix)
    {
        string suffix = objectName.Substring(prefix.Length);

        if (suffix.StartsWith("_"))
            suffix = suffix.Substring(1);

        return suffix.Trim();
    }

    private static string FormatSuffix(string suffix)
    {
        if (string.IsNullOrEmpty(suffix))
            return suffix;

        // 单个字符：字母转大写，数字保持原样
        if (suffix.Length == 1)
            return suffix.ToUpper();

        // 多字符：首字母大写，其余小写
        return char.ToUpper(suffix[0]) + suffix.Substring(1).ToLower();
    }
}