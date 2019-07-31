using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBehaviour : StateMachineBehaviour {

    private float movementSpeed = 6f;
    private Vector3 ground;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        ground = Vector3.zero;
    }

    private void SimulateFall(Transform transform) {
        ground.x = transform.position.x;
        ground.z = transform.position.z;

        if (Vector3.Distance(transform.position, ground) >= 0f) {
            transform.position = Vector3.MoveTowards(transform.position, ground, Time.deltaTime * movementSpeed);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        SimulateFall(animator.gameObject.transform);

        Ray ray = new Ray(animator.gameObject.transform.position, Vector3.down);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo)) {
            if (Vector3.Distance(animator.gameObject.transform.position, hitInfo.point) <= 0.2f) {
                animator.SetTrigger("hasLanded");  
            }
        }
    }
}
