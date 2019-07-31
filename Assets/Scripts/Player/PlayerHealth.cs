using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private UIManager uIManager;
    [SerializeField] GameObject playerRagdoll;

    private float health;

    private void Start() {
        health = maxHealth;
    }

    public float GetHealth() {
        return health;
    }

    public void TakeDamage(float amount) {
        if (health >= amount) {
            health -= amount;
        } else {
            health = 0f;
            Die();
        }
        uIManager.UpdatePlayerHealth(health);
    }

    private void Die() {
        // Instantiate Ragdoll
        Destroy(gameObject);
        Instantiate(playerRagdoll, transform.position, transform.rotation);
        uIManager.ShowGameOverUI();
#if UNITY_STANDALONE_WIN
        Cursor.visible = true;
#endif

    }
}
