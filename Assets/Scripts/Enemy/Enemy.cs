using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {


    [SerializeField] private float positionUpdateTime = 0.5f;
    [SerializeField] private float rageUpTimer = 4f;
    [SerializeField] private float rageUpSpeed = 7;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private Collider sordCollider;
    [SerializeField] private bool isReady = false;

    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHealth health;
    private Transform player;

    private bool isAttacking = false;
    private float rageUpTime = 0f;
    private float currentMovementSpeed;
    private float movementSpeed;
    private float currentTime = 0f;
    private bool canPlayAttackSound = false;


    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>().transform;
        agent = GetComponent<NavMeshAgent>();
        movementSpeed = agent.speed;
        currentMovementSpeed = movementSpeed;

        OnEnable();
    }

    private void OnEnable() {
        if (agent != null) {
            agent.enabled = false;  // If it's enabled NavMesh will have some problem
        }
    }


    // Update is called once per frame
    void Update() {
        if (isReady) {


            if (player != null) { // Player is alive

                UpdateDestination();

                CheckIfAttacking();

                // If enemy is not near player for a long time, enemy speed will be increased
                RageUp();

                // If distance between player is less that 2.5 units, enemy will attack
                if (agent.remainingDistance <= 2.5f) {
                    agent.speed = 0;
                    Attack();
                    isAttacking = true;
                    transform.LookAt(player.transform);
                }

                if (!isAttacking) {
                    agent.speed = currentMovementSpeed;
                    StopAttack();
                }
            } else {
                // If player not alive, destroy enemy
                Destroy(gameObject, 0.5f);
            }
        }
    }

    public void SetEnemyReady(bool state) {
        agent.enabled = true;

        if (player != null) {
            // Update destination for the enemy to player position
            agent.SetDestination(player.position);

        }
        isReady = state;
    }

    private void RageUp() {
        rageUpTime += Time.deltaTime;

        if (rageUpTime >= rageUpTimer) {
            rageUpTime = 0f;
            currentMovementSpeed = rageUpSpeed;
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
        sordCollider.enabled = false;
        animator.SetBool("isAttacking", false);
    }

    // Play attack sound only when the enemy is in attack animation and when the enemy hits the player
    public void CanPlaySound(int state) {
        if (state == 1) {
            canPlayAttackSound = true;
        } else {
            canPlayAttackSound = false;
        }
    }

    private void Attack() {
        sordCollider.enabled = true;    // Collider will be disable to prevent unnecessary collissions 
        animator.SetBool("isAttacking", true);
    }

    private void UpdateDestination() {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f) {
            currentTime = positionUpdateTime;

            Vector3 destination = InterceptPlayer();

            agent.SetDestination(destination);
        }

    }

    private Vector3 InterceptPlayer() {
        Vector3 playerVelocity = player.GetComponent<NavMeshAgent>().velocity;
        Vector3 relativeVelocity = playerVelocity - agent.velocity;
        Vector3 relativePosition = player.transform.position - transform.position;

        float timeToClose = relativePosition.magnitude / relativeVelocity.magnitude;
        return player.transform.position + (playerVelocity * timeToClose);
    }

    public void InstantiateBlood(Vector3 position) {
        GameObject blood = Instantiate(bloodPrefab, position, Quaternion.identity);
        Destroy(blood, 2f);
    }

    public bool GetCanPlayAttackSound() {
        return canPlayAttackSound;
    }

}
