using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// Controls the cashier NavMesh walking sequence.
/// The cashier follows the user's position point, enters StandAtUser after first arrival,
/// keeps following the user while in StandAtUser, and only leaves that state when externally triggered.
/// Object switches such as board visibility and paper visibility should be handled through UnityEvents.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class NMAWalkModified : MonoBehaviour
{
    private enum CashierWalkState
    {
        Idle,
        WalkToUser,
        StandAtUser,
        WalkToPaper,
        WalkToTypewriter,
        StandAtTypewriter
    }

    [Header("Navigation Targets")]
    [Tooltip("Moving position point attached to or placed near the user. The cashier keeps following this point during StandAtUser.")]
    public Transform UserPositionPoint;

    [Tooltip("Target point where the cashier walks to collect or reach the paper.")]
    public Transform PaperTargetPoint;

    [Tooltip("Target point where the cashier stops near the typewriter.")]
    public Transform TypewriterTargetPoint;

    [Header("Movement Settings")]
    [Tooltip("Normal walking speed used before carrying paper.")]
    public float NormalWalkSpeed = 1.8f;

    [Tooltip("Slower walking speed used when walking with paper.")]
    public float PaperWalkSpeed = 0.8f;

    [Tooltip("Extra distance added to the NavMeshAgent stopping distance when checking arrival.")]
    public float ArriveTolerance = 0.15f;

    [Header("Events")]
    [Tooltip("Called once when the cashier first reaches the user and enters StandAtUser.")]
    public UnityEvent OnStandAtUser;

    [Tooltip("Called when an external interaction ends StandAtUser and the cashier starts walking to the paper.")]
    public UnityEvent OnLeaveUser;

    [Tooltip("Called when the cashier arrives at the paper point.")]
    public UnityEvent OnArrivePaper;

    [Tooltip("Called when the cashier starts walking toward the typewriter with paper.")]
    public UnityEvent OnWalkWithPaper;

    [Tooltip("Called when the cashier arrives at the typewriter and enters StandAtTypewriter.")]
    public UnityEvent OnStandAtTypewriter;

    [Header("Graphics")]
    [Tooltip("Animator used by the cashier model.")]
    public Animator AvatarAnimator;

    [Tooltip("Animator float parameter used to switch idle, normal walk, and paper walk.")]
    public string SpeedString = "speed";

    [Header("System Stuff")]
    [Tooltip("The NavMeshAgent used to move the cashier.")]
    public NavMeshAgent MyNma;

    [Tooltip("Current cashier walking state. Visible for debugging.")]
    [SerializeField] private CashierWalkState currentState = CashierWalkState.Idle;

    private Transform currentTarget;

    private void Awake()
    {
        MyNma = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        WalkToUser();
    }

    private void Update()
    {
        UpdateMovingTarget();
        UpdateState();
        HandleAvatar();
    }

    /// <summary>
    /// Starts the first movement.
    /// The cashier follows the user's moving position point.
    /// </summary>
    public void WalkToUser()
    {
        MoveToTarget(UserPositionPoint, NormalWalkSpeed, CashierWalkState.WalkToUser);
    }

    /// <summary>
    /// External trigger used to end StandAtUser.
    /// Connect InteractableGeneral.onPrimaryInteract or onPrimaryInteractLift to this method.
    /// </summary>
    public void ContinueFromUser()
    {
        if (currentState != CashierWalkState.StandAtUser) return;

        OnLeaveUser.Invoke();
        MoveToTarget(PaperTargetPoint, NormalWalkSpeed, CashierWalkState.WalkToPaper);
    }

    /// <summary>
    /// Starts walking to the typewriter using the paper walking speed.
    /// </summary>
    private void WalkToTypewriter()
    {
        OnWalkWithPaper.Invoke();
        MoveToTarget(TypewriterTargetPoint, PaperWalkSpeed, CashierWalkState.WalkToTypewriter);
    }

    /// <summary>
    /// Sets the current target, speed, and walking state.
    /// The actual destination is refreshed every frame by UpdateMovingTarget().
    /// </summary>
    private void MoveToTarget(Transform target, float speed, CashierWalkState nextState)
    {
        if (target == null)
        {
            Debug.LogWarning("NMAWalkModified: Target point is not assigned.");
            return;
        }

        currentTarget = target;

        MyNma.speed = speed;
        MyNma.isStopped = false;
        MyNma.SetDestination(currentTarget.position);

        currentState = nextState;
    }

    /// <summary>
    /// Keeps the NavMeshAgent destination locked to the current target Transform.
    /// This allows the cashier to keep following the moving user position point during StandAtUser.
    /// </summary>
    private void UpdateMovingTarget()
    {
        if (currentTarget == null) return;
        if (MyNma.isStopped) return;

        MyNma.SetDestination(currentTarget.position);
    }

    /// <summary>
    /// Updates state transitions after the cashier reaches each target.
    /// </summary>
    private void UpdateState()
    {
        switch (currentState)
        {
            case CashierWalkState.Idle:
                break;

            case CashierWalkState.WalkToUser:
                if (HasArrived())
                {
                    currentState = CashierWalkState.StandAtUser;
                    OnStandAtUser.Invoke();
                }
                break;

            case CashierWalkState.StandAtUser:
                break;

            case CashierWalkState.WalkToPaper:
                if (HasArrived())
                {
                    StopAgent();
                    OnArrivePaper.Invoke();
                    WalkToTypewriter();
                }
                break;

            case CashierWalkState.WalkToTypewriter:
                if (HasArrived())
                {
                    StopAgent();
                    currentState = CashierWalkState.StandAtTypewriter;
                    OnStandAtTypewriter.Invoke();
                }
                break;

            case CashierWalkState.StandAtTypewriter:
                break;
        }
    }

    /// <summary>
    /// Stops the NavMeshAgent and clears the current path.
    /// This is only used for fixed destinations, not for StandAtUser.
    /// </summary>
    private void StopAgent()
    {
        MyNma.isStopped = true;
        MyNma.ResetPath();
        currentTarget = null;
    }

    /// <summary>
    /// Checks whether the cashier has arrived at the current destination.
    /// </summary>
    private bool HasArrived()
    {
        if (MyNma.pathPending) return false;
        if (MyNma.remainingDistance > MyNma.stoppingDistance + ArriveTolerance) return false;
        if (MyNma.velocity.sqrMagnitude > 0.01f) return false;

        return true;
    }

    /// <summary>
    /// Sends the NavMeshAgent movement speed to the Animator.
    /// The Animator uses this speed value to switch between standing, normal walking, and paper walking.
    /// </summary>
    private void HandleAvatar()
    {
        if (AvatarAnimator == null) return;

        float currentSpeed = MyNma.velocity.magnitude;
        AvatarAnimator.SetFloat(SpeedString, currentSpeed);
    }
}