using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {


    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] private LineRenderer lineRenderer;

#if UNITY_ANDROID
    [SerializeField] private FixedJoystick joystick;
#endif

    private Animator animator;
    private Crosshair crosshair;

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
    private float x;
    private float z;

    void Start() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        crosshair = FindObjectOfType<Crosshair>();

        newPosition = Vector3.zero;
        movementDirection = Vector3.zero;
        lookAtObject = Vector3.forward;
        aimingDirection = Vector3.zero;

        animationStates = new Dictionary<string, bool>();

        AddAnimationStatesToDictionary();
    }

    private void AddAnimationStatesToDictionary() {
        animationStates.Add(IS_RUNNING_FORWARD, false);
        animationStates.Add(IS_RUNNING_RIGHT, false);
        animationStates.Add(IS_RUNNING_LEFT, false);
        animationStates.Add(IS_RUNNING_BACKWARD, false);
        animationStates.Add(IS_IDLE, false);
    }

    void Update() {
        LookAtCrosshair();

#if UNITY_ANDROID

        Move();


#else
        // Get mouse positio nand move the player to that position
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

#endif
        CheckDestinationReach(newPosition);
    }

    private void Move() {
        x = joystick.Horizontal;
        z = joystick.Vertical;
        newPosition.x = x;
        newPosition.z = z;

        if (x != 0f || z != 0f) {
            Debug.Log(newPosition);
            agent.SetDestination(transform.position + newPosition.normalized);
        }

    }

    private void LookAtCrosshair() {

        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit)) {
            if (Vector3.Distance(transform.position, raycastHit.point) > 0.5f) {
                lookAtObject.x = raycastHit.point.x;
                lookAtObject.z = raycastHit.point.z;
                transform.LookAt(lookAtObject);
            }
        }
    }

    private Vector3 GetPositionFromMousePoint() {
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo)) {
            return hitInfo.point;
        }
        return transform.position;
    }

    private void CheckDestinationReach(Vector3 targetPosition) {
#if UNITY_ANDROID
        if (Vector3.Distance(transform.position, transform.position + targetPosition) >= 0.3f) {
            AnimateMovement(newPosition + transform.position);
        } else {
            DisableOtherAnimation(IS_IDLE);
        }

#else

        if (agent.remainingDistance >= 0.3f) {
            AnimateMovement(newPosition);
        } else {
            DisableOtherAnimation(IS_IDLE);
            // Disable path line renderer
            lineRenderer.positionCount = 0;
        }

#endif
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
