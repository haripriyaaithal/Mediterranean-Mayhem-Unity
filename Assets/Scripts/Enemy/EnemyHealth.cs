using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private Image healthBar;

    private SoundManager soundManager;

    private float currentHealth = 0;

    private void Start() {
        currentHealth = maxHealth;
        soundManager = FindObjectOfType<SoundManager>();
    }

    public float GetHealth() {
        return currentHealth;
    }

    public void TakeDamage(float amount) {
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth >= amount) {
            currentHealth -= amount;
        } else {
            currentHealth = 0f;
            Die();
        }
    }

    private void Die() {
        Instantiate(ragdollPrefab, transform.position, transform.rotation);
        soundManager.PlayEnemyDeath();
        MoveEnemyToPool();
    }

    private void MoveEnemyToPool() {
        ResetHealth();
        FindObjectOfType<EnemySpawner>().AddToQueue(gameObject);
    }

    public void ResetHealth() {
        healthBar.fillAmount = 1f;
        currentHealth = maxHealth;
    }
}
