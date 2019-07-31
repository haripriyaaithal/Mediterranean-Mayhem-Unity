using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySord : MonoBehaviour {

    [SerializeField] private float damage = 7f;

    private Player player;
    private PlayerHealth playerHealth;
    private Enemy enemy;
    private SoundManager soundManager;

    private void Start() {
        player = FindObjectOfType<Player>();
        soundManager = FindObjectOfType<SoundManager>();
        playerHealth = player.GetComponent<PlayerHealth>();
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other) {
        if (enemy.GetCanPlayAttackSound()) { // If the collision during attack
            if (other.gameObject.CompareTag("Player")) {
                playerHealth.TakeDamage(damage);
                soundManager.PlayEnemyAttack();
            }
        }
    }
}