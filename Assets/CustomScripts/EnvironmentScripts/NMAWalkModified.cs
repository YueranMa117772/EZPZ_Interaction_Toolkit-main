using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// Controls the cashier walking sequence.
/// The cashier walks to the user position point, faces the user while walking and standing,
/// then leaves only when ContinueFromUser is called.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class NMAWalkModified : MonoBehaviour
{
    private enum CashierWalkState
    {
        WalkToUser,
        StandAtUser,
        WalkToPaper,
        WalkToTypewriter,
        StandAtTypewriter
    }

    [Header("Targets")]
    [Tooltip("Position point the cashier walks to near the user.")]
    public Transform UserPositionPoint;

    [Tooltip("Target the cashier faces while walking to and standing at the user. Usually the user body, head, XR camera, or player root.")]
    public Transform UserLookTarget;

    [Tooltip("Position point where the cashier walks to reach the paper.")]
    public Transform PaperTargetPoint;

    [Tooltip("Position point where the cashier walks to reach the typewriter.")]
    public Transform TypewriterTargetPoint;

    [Header("User Control")]
    [Tooltip("FirstPersonController used to lock user movement while standing at the user.")]
    public FirstPersonController UserController;

    [Header("Movement")]
    [Tooltip("Rotation speed used when facing the user.")]
    public float StandFaceRotationSpeed = 8f;

    [Header("Events")]
    [Tooltip("Called once when the cashier reaches the user and enters StandAtUser.")]
    public UnityEvent OnStandAtUser;

    [Tooltip("Called when ContinueFromUser is called and the cashier leaves the user.")]
    public UnityEvent OnLeaveUser;

    [Tooltip("Called when the cashier reaches the paper point.")]
    public UnityEvent OnArrivePaper;

    [Tooltip("Called when the cashier reaches the typewriter point.")]
    public UnityEvent OnStandAtTypewriter;

    [Header("Animator")]
    [Tooltip("Animator used by the cashier model.")]
    public Animator AvatarAnimator;

    [Tooltip("Animator float parameter used for idle/walk switching.")]
    public string SpeedString = "speed";

    [Tooltip("Animator bool parameter used for normal walk / paper walk switching.")]
    public string WithPaperString = "withPaper";

    [Header("State")]
    [Tooltip("Current walking state. Visible for checking the sequence in Play Mode.")]
    [SerializeField] private CashierWalkState currentState;

    private NavMeshAgent myNma;
    private Transform currentTarget;

    private void Awake()
    {
        myNma = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SetPaperMode(false);
        MoveTo(UserPositionPoint, CashierWalkState.WalkToUser);
    }

    private void Update()
    {
        UpdateTargetPosition();
        UpdateState();
        UpdateUserFacing();
        UpdateAnimator();
    }

    /// <summary>
    /// Ends StandAtUser and starts walking to the paper point.
    /// This should be called by an external interaction event.
    /// </summary>
    public void ContinueFromUser()
    {
        if (currentState != CashierWalkState.StandAtUser) return;

        SetUserMovementLocked(false);
        OnLeaveUser.Invoke();

        MoveTo(PaperTargetPoint, CashierWalkState.WalkToPaper);
    }

    /// <summary>
    /// Starts moving to a target.
    /// Paper point is treated as a passing point, so auto braking is disabled there.
    /// </summary>
    private void MoveTo(Transform target, CashierWalkState nextState)
    {
        if (target == null) return;

        currentTarget = target;
        currentState = nextState;

        myNma.isStopped = false;

        // WalkToUser uses custom facing, so NavMeshAgent must not rotate the model.
        myNma.updateRotation = nextState != CashierWalkState.WalkToUser;

        myNma.autoBraking = nextState != CashierWalkState.WalkToPaper;
        myNma.SetDestination(currentTarget.position);
    }

    /// <summary>
    /// Keeps the destination locked to the current target.
    /// This allows the cashier to keep following the moving user position point before arriving.
    /// </summary>
    private void UpdateTargetPosition()
    {
        if (currentTarget == null) return;
        if (myNma.isStopped) return;

        myNma.SetDestination(currentTarget.position);
    }

    /// <summary>
    /// Handles state changes after reaching each target.
    /// </summary>
    private void UpdateState()
    {
        switch (currentState)
        {
            case CashierWalkState.WalkToUser:
                if (!HasStoppedAtTarget()) return;

                StopAgent();
                currentState = CashierWalkState.StandAtUser;

                myNma.updateRotation = false;

                SetUserMovementLocked(true);
                OnStandAtUser.Invoke();
                break;

            case CashierWalkState.WalkToPaper:
                if (!HasReachedDistance()) return;

                OnArrivePaper.Invoke();
                SetPaperMode(true);
                MoveTo(TypewriterTargetPoint, CashierWalkState.WalkToTypewriter);
                break;

            case CashierWalkState.WalkToTypewriter:
                if (!HasStoppedAtTarget()) return;

                StopAgent();
                SetPaperMode(false);

                currentState = CashierWalkState.StandAtTypewriter;
                OnStandAtTypewriter.Invoke();
                break;
        }
    }

    /// <summary>
    /// Faces the user while walking to the user and while standing at the user.
    /// </summary>
    private void UpdateUserFacing()
    {
        if (currentState != CashierWalkState.WalkToUser &&
            currentState != CashierWalkState.StandAtUser)
        {
            return;
        }

        Transform target = UserLookTarget != null ? UserLookTarget : UserPositionPoint;
        FaceTarget(target);
    }

    /// <summary>
    /// Rotates this object toward a target on the horizontal plane.
    /// </summary>
    private void FaceTarget(Transform target)
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            StandFaceRotationSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Locks or unlocks user movement while keeping camera rotation available.
    /// </summary>
    private void SetUserMovementLocked(bool locked)
    {
        if (UserController == null) return;

        UserController.SetMovementLocked(locked);
    }

    /// <summary>
    /// Checks whether the NavMeshAgent is within stopping distance of the current destination.
    /// Used for passing through the paper point without waiting for full stop.
    /// </summary>
    private bool HasReachedDistance()
    {
        if (myNma.pathPending) return false;
        if (myNma.remainingDistance > myNma.stoppingDistance) return false;

        return true;
    }

    /// <summary>
    /// Checks whether the NavMeshAgent has reached and stopped at the current destination.
    /// Used for final standing states.
    /// </summary>
    private bool HasStoppedAtTarget()
    {
        if (!HasReachedDistance()) return false;
        if (myNma.velocity.sqrMagnitude > 0.01f) return false;

        return true;
    }

    /// <summary>
    /// Stops movement and clears the current target.
    /// </summary>
    private void StopAgent()
    {
        myNma.isStopped = true;
        myNma.ResetPath();
        currentTarget = null;
    }

    /// <summary>
    /// Sets the Animator paper state.
    /// </summary>
    private void SetPaperMode(bool hasPaper)
    {
        if (AvatarAnimator == null) return;

        AvatarAnimator.SetBool(WithPaperString, hasPaper);
    }

    /// <summary>
    /// Sends NavMeshAgent movement speed to the Animator.
    /// </summary>
    private void UpdateAnimator()
    {
        if (AvatarAnimator == null) return;

        AvatarAnimator.SetFloat(SpeedString, myNma.velocity.magnitude);
    }
}