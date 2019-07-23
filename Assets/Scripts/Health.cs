using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] private float health;
    [SerializeField] GameObject ragdollPrefab;

    public float GetHealth() {
        return health;
    }

    public void TakeDamage(float amount, Vector3 bulletVelocity, Vector3 hitPoint) {
        if (health >= amount) {
            health -= amount;
        } else {
            health = 0f;
            Die(bulletVelocity, hitPoint);
        }
    }

    public void AddHealth(float amount) {
        health += amount;
    }

    private void Die(Vector3 bulletVelocity, Vector3 hitPoint) {
        Debug.Log("HitPoint: " + hitPoint);
        Debug.Log("BulletVelocity: " + bulletVelocity);
        Instantiate(ragdollPrefab, transform.position, transform.rotation);
        /*
        Rigidbody rb = enemy.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;
        */
        // enemy.GetComponent<Rigidbody>().AddForceAtPosition(bulletVelocity * 1000, hitPoint, ForceMode.Impulse);

        Debug.Log("Added force");
        Destroy(gameObject);
    }
}
