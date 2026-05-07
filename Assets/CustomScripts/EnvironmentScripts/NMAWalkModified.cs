using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// Controls the cashier walking sequence.
[RequireComponent(typeof(NavMeshAgent))]
public class NMAWalkModified : MonoBehaviour
{
    private enum CashierWalkState
    {
        StandAtGameStart,
        WalkToUser,
        StandAtUser,
        WalkToPaper,
        WalkToTypewriter,
        StandAtTypewriter
    }

    public Transform UserPositionPoint;
    public Transform UserLookTarget;
    public Transform PaperTargetPoint;
    public Transform TypewriterTargetPoint;

    public FirstPersonController UserController;

    public float StandFaceRotationSpeed = 8f;
    public float LeaveUserDelay = 1f;

    [Header("Events")]
    public UnityEvent OnStandAtGameStart;
    public UnityEvent OnStandAtUser;
    public UnityEvent OnLeaveUser;
    public UnityEvent OnArrivePaper;
    public UnityEvent OnStandAtTypewriter;

    public Animator AvatarAnimator;
    public string SpeedString = "speed";
    public string WithPaperString = "withPaper";

    [SerializeField] private CashierWalkState currentState;

    private NavMeshAgent myNma;
    private Transform currentTarget;
    private bool isLeavingUser;

    private void Awake()
    {
        myNma = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SetPaperMode(false);
        StopAgent();
        currentState = CashierWalkState.StandAtGameStart;
        OnStandAtGameStart.Invoke();
    }

    private void Update()
    {
        UpdateTargetPosition();
        UpdateState();
        UpdateUserFacing();
        UpdateAnimator();
    }

    public void ContinueFromGameStart()
    {
        if (currentState != CashierWalkState.StandAtGameStart) return;

        MoveTo(UserPositionPoint, CashierWalkState.WalkToUser);
    }

    public void ContinueFromUser()
    {
        if (currentState != CashierWalkState.StandAtUser) return;
        if (isLeavingUser) return;

        StartCoroutine(LeaveUserAfterDelay());
    }

    private IEnumerator LeaveUserAfterDelay()
    {
        isLeavingUser = true;

        yield return new WaitForSeconds(LeaveUserDelay);

        if (UserController != null)
            UserController.SetMovementLocked(false);

        OnLeaveUser.Invoke();

        MoveTo(PaperTargetPoint, CashierWalkState.WalkToPaper);

        isLeavingUser = false;
    }

    private void MoveTo(Transform target, CashierWalkState nextState)
    {
        if (target == null) return;

        currentTarget = target;
        currentState = nextState;

        myNma.isStopped = false;
        myNma.updateRotation = nextState != CashierWalkState.WalkToUser;
        myNma.autoBraking = nextState != CashierWalkState.WalkToPaper;
        myNma.SetDestination(target.position);
    }

    private void UpdateTargetPosition()
    {
        if (currentTarget == null) return;
        if (myNma.isStopped) return;

        myNma.SetDestination(currentTarget.position);
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case CashierWalkState.WalkToUser:
                if (!Stopped()) return;

                StopAgent();
                currentState = CashierWalkState.StandAtUser;
                myNma.updateRotation = false;

                if (UserController != null)
                    UserController.SetMovementLocked(true);

                OnStandAtUser.Invoke();
                break;

            case CashierWalkState.WalkToPaper:
                if (!Reached()) return;

                OnArrivePaper.Invoke();
                SetPaperMode(true);
                MoveTo(TypewriterTargetPoint, CashierWalkState.WalkToTypewriter);
                break;

            case CashierWalkState.WalkToTypewriter:
                if (!Stopped()) return;

                StopAgent();
                SetPaperMode(false);

                currentState = CashierWalkState.StandAtTypewriter;
                OnStandAtTypewriter.Invoke();
                break;
        }
    }

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

    private bool Reached()
    {
        return !myNma.pathPending &&
               myNma.remainingDistance <= myNma.stoppingDistance;
    }

    private bool Stopped()
    {
        return Reached() &&
               myNma.velocity.sqrMagnitude <= 0.01f;
    }

    private void StopAgent()
    {
        myNma.isStopped = true;
        myNma.ResetPath();
        currentTarget = null;
    }

    private void SetPaperMode(bool hasPaper)
    {
        if (AvatarAnimator == null) return;

        AvatarAnimator.SetBool(WithPaperString, hasPaper);
    }

    private void UpdateAnimator()
    {
        if (AvatarAnimator == null) return;

        AvatarAnimator.SetFloat(SpeedString, myNma.velocity.magnitude);
    }
}