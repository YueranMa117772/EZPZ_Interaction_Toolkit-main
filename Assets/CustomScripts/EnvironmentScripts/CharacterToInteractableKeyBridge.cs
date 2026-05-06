using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Receives characters from BoardTextAutoPlayer,
/// finds the matching BoxCollider assigned in the Inspector,
/// and moves the NPC finger target:
/// move on XZ plane, press vertically, release vertically.
/// </summary>
public class CharacterToInteractableKeyBridge : MonoBehaviour
{
    [Serializable]
    public class CharacterKeyBinding
    {
        [Tooltip("Character sent by BoardTextAutoPlayer. Example: a, b, 1, ., ,")]
        public char Character;

        [Tooltip("The matching BoxCollider used as the target area for this character.")]
        public BoxCollider KeyCollider;
    }

    [Header("Target Point")]
    [Tooltip("The IK target, hand target, or finger target that should move to the matched collider center.")]
    public Transform TargetPoint;

    [Header("Key Press Group")]
    [Tooltip("Shared key press settings used to synchronize the NPC finger with key movement.")]
    public KeyPressGroup KeyGroup;

    [Header("Action Speed")]
    [Tooltip("Speed used to move the target point on the XZ plane. Y height stays unchanged.")]
    [Min(0.001f)]
    public float MoveSpeed = 0.5f;

    [Header("Character To Collider Bindings")]
    [Tooltip("Assign each playable character to its matching BoxCollider.")]
    public List<CharacterKeyBinding> Bindings = new List<CharacterKeyBinding>();

    private Dictionary<char, CharacterKeyBinding> bindingLookup;

    private void Awake()
    {
        BuildLookup();
    }

    /// <summary>
    /// Builds the character lookup dictionary from the Inspector bindings.
    /// </summary>
    private void BuildLookup()
    {
        bindingLookup = new Dictionary<char, CharacterKeyBinding>();

        foreach (CharacterKeyBinding binding in Bindings)
        {
            if (binding.KeyCollider == null) continue;

            char key = NormalizeCharacter(binding.Character);

            if (!bindingLookup.ContainsKey(key))
            {
                bindingLookup.Add(key, binding);
            }
            else
            {
                Debug.LogWarning("Duplicate character binding ignored: " + binding.Character, this);
            }
        }
    }

    /// <summary>
    /// Plays one full character action.
    /// Step 1: Move only on XZ plane, keeping the current Y height.
    /// Step 2: Press only on Y axis, keeping XZ fixed.
    /// Step 3: Release only on Y axis, returning to the original Y height.
    /// </summary>
    public IEnumerator PlayCharacter(char c)
    {
        if (bindingLookup == null)
        {
            BuildLookup();
        }

        char key = NormalizeCharacter(c);

        if (!bindingLookup.TryGetValue(key, out CharacterKeyBinding binding))
        {
            Debug.LogWarning("No collider binding found for character: " + c, this);
            yield break;
        }

        if (TargetPoint == null)
        {
            Debug.LogWarning("TargetPoint is not assigned.", this);
            yield break;
        }

        if (KeyGroup == null)
        {
            Debug.LogWarning("KeyGroup is not assigned.", this);
            yield break;
        }

        if (binding.KeyCollider == null)
        {
            Debug.LogWarning("KeyCollider is missing for character: " + c, this);
            yield break;
        }

        Vector3 colliderCenter = binding.KeyCollider.bounds.center;

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

    /// <summary>
    /// Calculates the NPC finger press speed so its vertical press duration matches the shared key press duration.
    /// </summary>
    private float GetSyncedPressSpeed(Vector3 startPosition, Vector3 endPosition)
    {
        float fingerDistance = Mathf.Abs(startPosition.y - endPosition.y);
        float keyPressTime = KeyGroup.GetPressTime();

        return fingerDistance / keyPressTime;
    }

    /// <summary>
    /// Calculates the NPC finger release speed so its vertical release duration matches the shared key return duration.
    /// </summary>
    private float GetSyncedReleaseSpeed(Vector3 startPosition, Vector3 endPosition)
    {
        float fingerDistance = Mathf.Abs(startPosition.y - endPosition.y);
        float keyReturnTime = KeyGroup.GetReturnTime();

        return fingerDistance / keyReturnTime;
    }

    /// <summary>
    /// Moves the target point only on the XZ plane.
    /// Y height is taken from the start position and remains unchanged.
    /// </summary>
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

    /// <summary>
    /// Moves the target point only vertically.
    /// XZ position is taken from the start position and remains unchanged.
    /// </summary>
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

    /// <summary>
    /// Normalizes letters so A and a always use the same collider.
    /// </summary>
    private char NormalizeCharacter(char c)
    {
        return char.ToLowerInvariant(c);
    }
}