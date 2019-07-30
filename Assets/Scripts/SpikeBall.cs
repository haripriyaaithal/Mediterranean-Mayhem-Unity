using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour {

    [SerializeField] private float enemyDamage = 40f;

    private Rigidbody rigidbody;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (!rigidbody.IsSleeping()) {  // Sphere should be moving
            if (collision.gameObject.CompareTag("Enemy")) {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(enemyDamage);
            }
        }
    }

}
