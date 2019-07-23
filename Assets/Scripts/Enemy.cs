using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    [SerializeField] private Transform player;
    [SerializeField] private float positionUpdateTime = 0.5f;
    [SerializeField] private float rageUpTimer = 4f;
    [SerializeField] private float rageUpSpeed = 6;
    [SerializeField] GameObject bloodPrefab;

    private NavMeshAgent agent;
    private float currentTime = 0f;
    private Animator animator;
    private float movementSpeed;
    private Health health;
    private bool isAttacking = false;
    private float rageUpTime = 0f;
    private float currentMovementSpeed;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //health = GetComponent<Health>();

        movementSpeed = agent.speed;
        currentMovementSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update() {
        UpdateDestination();

        CheckIfAttacking();

        RageUp();

        if (agent.remainingDistance <= 2f) {
            agent.speed = 0;
            Attack();
            isAttacking = true;
        }

        if (!isAttacking) {
            agent.speed = currentMovementSpeed;
            StopAttack();
        }

    }

    private void RageUp() {
        rageUpTime += Time.deltaTime;

        if (rageUpTime >= rageUpTimer) {
            rageUpTime = 0f;
            currentMovementSpeed = rageUpSpeed;
            Debug.Log("Raged UP!");
        }

    }

    private void RageDown() {
        rageUpTime = 0f;
        currentMovementSpeed = movementSpeed;
    }

    private void CheckIfAttacking() {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Swiping")) {
            isAttacking = true;
            RageDown();
        } else {
            isAttacking = false;
        }
    }

    public void StopAttack() {
        animator.SetBool("isAttacking", false);
    }

    private void Attack() {
        animator.SetBool("isAttacking", true);
    }

    private void UpdateDestination() {
        currentTime += Time.deltaTime;

        if (currentTime >= positionUpdateTime) {
            currentTime = 0;
            agent.SetDestination(player.position);
        }
    }

    public void InstantiateBlood(Vector3 position) {
        Instantiate(bloodPrefab, position, Quaternion.identity);
    }
}
