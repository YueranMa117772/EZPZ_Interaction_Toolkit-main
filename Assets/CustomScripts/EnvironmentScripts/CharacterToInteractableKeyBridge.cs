using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Moves NPC target to typed keys.
public class CharacterToInteractableKeyBridge : MonoBehaviour
{
    [Serializable]
    public class CharacterKeyBinding
    {
        public string Character = "";
        public BoxCollider KeyCollider;
    }

    public Transform TargetPoint;
    public KeyPressGroup KeyGroup;

    [Min(0.001f)]
    public float MoveSpeed = 0.5f;

    [Header("Bindings")]
    public List<CharacterKeyBinding> Bindings = new List<CharacterKeyBinding>();

    private Dictionary<char, BoxCollider> bindingLookup;

    private void Awake()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        bindingLookup = new Dictionary<char, BoxCollider>();

        foreach (CharacterKeyBinding binding in Bindings)
        {
            if (string.IsNullOrEmpty(binding.Character)) continue;
            if (binding.KeyCollider == null) continue;

            char key = GetBindingCharacter(binding.Character);

            if (!bindingLookup.ContainsKey(key))
            {
                bindingLookup.Add(key, binding.KeyCollider);
            }
        }
    }

    private char GetBindingCharacter(string text)
    {
        if (text == "\\n")
        {
            return '\n';
        }

        return NormalizeCharacter(text[0]);
    }

    public IEnumerator PlayCharacter(char c)
    {
        if (bindingLookup == null)
        {
            BuildLookup();
        }

        if (TargetPoint == null) yield break;
        if (KeyGroup == null) yield break;

        char key = NormalizeCharacter(c);

        if (!bindingLookup.TryGetValue(key, out BoxCollider keyCollider))
        {
            yield break;
        }

        Vector3 colliderCenter = keyCollider.bounds.center;
        float originalHeightY = TargetPoint.position.y;

        Vector3 moveStartPosition = TargetPoint.position;
        Vector3 moveEndPosition = new Vector3(
            colliderCenter.x,
            originalHeightY,
            colliderCenter.z
        );

        Vector3 pressStartPosition = moveEndPosition;
        Vector3 pressEndPosition = new Vector3(
            moveEndPosition.x,
            colliderCenter.y,
            moveEndPosition.z
        );

        Vector3 releaseStartPosition = pressEndPosition;
        Vector3 releaseEndPosition = new Vector3(
            moveEndPosition.x,
            originalHeightY,
            moveEndPosition.z
        );

        float pressSpeed = GetSyncedPressSpeed(pressStartPosition, pressEndPosition);
        float releaseSpeed = GetSyncedReleaseSpeed(releaseStartPosition, releaseEndPosition);

        yield return MoveTargetOnPlane(moveStartPosition, moveEndPosition, MoveSpeed);
        yield return MoveTargetVertical(pressStartPosition, pressEndPosition, pressSpeed);
        yield return MoveTargetVertical(releaseStartPosition, releaseEndPosition, releaseSpeed);
    }

    private float GetSyncedPressSpeed(Vector3 startPosition, Vector3 endPosition)
    {
        float fingerDistance = Mathf.Abs(startPosition.y - endPosition.y);
        float keyPressTime = KeyGroup.GetPressTime();

        return fingerDistance / keyPressTime;
    }

    private float GetSyncedReleaseSpeed(Vector3 startPosition, Vector3 endPosition)
    {
        float fingerDistance = Mathf.Abs(startPosition.y - endPosition.y);
        float keyReturnTime = KeyGroup.GetReturnTime();

        return fingerDistance / keyReturnTime;
    }

    private IEnumerator MoveTargetOnPlane(Vector3 startPosition, Vector3 endPosition, float speed)
    {
        if (TargetPoint == null) yield break;

        endPosition.y = startPosition.y;
        TargetPoint.position = startPosition;

        while (Vector3.Distance(TargetPoint.position, endPosition) > 0.0001f)
        {
            Vector3 currentPosition = Vector3.MoveTowards(
                TargetPoint.position,
                endPosition,
                speed * Time.deltaTime
            );

            currentPosition.y = startPosition.y;
            TargetPoint.position = currentPosition;

            yield return null;
        }

        TargetPoint.position = endPosition;
    }

    private IEnumerator MoveTargetVertical(Vector3 startPosition, Vector3 endPosition, float speed)
    {
        if (TargetPoint == null) yield break;

        endPosition.x = startPosition.x;
        endPosition.z = startPosition.z;
        TargetPoint.position = startPosition;

        while (Mathf.Abs(TargetPoint.position.y - endPosition.y) > 0.0001f)
        {
            Vector3 currentPosition = Vector3.MoveTowards(
                TargetPoint.position,
                endPosition,
                speed * Time.deltaTime
            );

            currentPosition.x = startPosition.x;
            currentPosition.z = startPosition.z;
            TargetPoint.position = currentPosition;

            yield return null;
        }

        TargetPoint.position = endPosition;
    }

    private char NormalizeCharacter(char c)
    {
        return char.ToLowerInvariant(c);
    }
}