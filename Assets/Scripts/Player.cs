using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {


    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] private LineRenderer lineRenderer;


    private Animator animator;

    private Vector3 newPosition;
    private Vector3 movementDirection;
    private Vector3 lookAtObject;
    private Vector3 aimingDirection;

    private Dictionary<string, bool> animationStates;

    private const string IS_RUNNING_FORWARD = "isRunning";
    private const string IS_RUNNING_RIGHT = "isRunningRight";
    private const string IS_RUNNING_LEFT = "isRunningLeft";
    private const string IS_RUNNING_BACKWARD = "isRunningBack";
    private const string IS_IDLE = "Idle";
    private const string IS_SHOOTING = "isShooting";

    private NavMeshAgent agent;

    private string previousTrigger;

    void Start() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        newPosition = Vector3.zero;
        movementDirection = Vector3.zero;
        lookAtObject = Vector3.forward;
        aimingDirection = Vector3.zero;

        animationStates = new Dictionary<string, bool>();

        animationStates.Add(IS_RUNNING_FORWARD, false);
        animationStates.Add(IS_RUNNING_RIGHT, false);
        animationStates.Add(IS_RUNNING_LEFT, false);
        animationStates.Add(IS_RUNNING_BACKWARD, false);
        animationStates.Add(IS_IDLE, false);
    }

    void Update() {

        if (Input.GetMouseButton(1)) {
            newPosition = GetPositionFromMousePoint();
            agent.SetDestination(newPosition);

            NavMeshPath navPath = agent.path;
            Vector3[] paths = navPath.corners;

            lineRenderer.positionCount = paths.Length;

            for (int path = 0; path < paths.Length; path++) {
                lineRenderer.SetPosition(path, paths[path]);
            }
        }

        CheckDestinationReach(newPosition);

        LookAtCrosshair();
    }

    private void LookAtCrosshair() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit)) {
            if (Vector3.Distance(transform.position, raycastHit.point) > 2f) {
                lookAtObject.x = raycastHit.point.x;
                lookAtObject.z = raycastHit.point.z;
                transform.LookAt(lookAtObject);
            }
        }
    }

    private Vector3 GetPositionFromMousePoint() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo)) {
            return hitInfo.point;
        }
        return transform.position;
    }

    private void CheckDestinationReach(Vector3 targetPosition) {

        Debug.DrawRay(transform.position, targetPosition - transform.position, Color.red);
        Debug.DrawRay(transform.position, GetPositionFromMousePoint() - transform.position, Color.green);

        if (agent.remainingDistance >= 0.3f) {
            //  transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            AnimateMovement(newPosition);
        } else {
            DisableOtherAnimation(IS_IDLE);
            // Disable path line renderer
            lineRenderer.positionCount = 0;
        }
    }

    private void AnimateMovement(Vector3 targetPosition) {

        movementDirection = targetPosition - transform.position;
        aimingDirection = GetPositionFromMousePoint() - transform.position;

        float dotProduct = Vector3.Dot(movementDirection.normalized, aimingDirection.normalized);
        float signedAngle = Vector3.SignedAngle(movementDirection, aimingDirection, Vector3.up);

        if (dotProduct > 0f) {
            if (dotProduct > 0.93f && dotProduct < 1f) {
                // Forward run
                DisableOtherAnimation(IS_RUNNING_FORWARD);
            } else if (signedAngle > 0f) {
                // Left run
                DisableOtherAnimation(IS_RUNNING_LEFT);
            } else if (signedAngle < 0f) {

                // Right run
                DisableOtherAnimation(IS_RUNNING_RIGHT);
            }
        } else if (dotProduct < 0f) {
            if (dotProduct < -0.8f && dotProduct > -1f) {
                // Backward run
                DisableOtherAnimation(IS_RUNNING_BACKWARD); // + -> right - -> left
            } else if (signedAngle > 0f) {
                // Left run
                DisableOtherAnimation(IS_RUNNING_LEFT);
            } else if (signedAngle < 0f) {

                // Right run
                DisableOtherAnimation(IS_RUNNING_RIGHT);
            }

        }

    }

    public void AnimatingShooting(bool state) {
        animator.SetBool(IS_SHOOTING, state);
    }

    private void DisableOtherAnimation(string animation) {
        if (!string.IsNullOrEmpty(previousTrigger)) {
            animator.ResetTrigger(previousTrigger);
        }
        animator.SetTrigger(animation);
        previousTrigger = animation;
    }

    public Vector3 GetPlayerPosition() {
        return transform.position;
    }

}
