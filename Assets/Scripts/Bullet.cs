using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    [SerializeField] private float force = 50f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private Rigidbody rigidbody;
    private Vector3 firingDirection;

    public void SetFiringDirection(Vector3 direction) {
        firingDirection = direction;
        AddForce();
    }

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();

    }

    private void AddForce() {
        rigidbody.AddForce(firingDirection * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage, GetVelocity(), collision.contacts[0].point);
            collision.gameObject.GetComponent<Enemy>().InstantiateBlood(collision.contacts[0].point);
        }
    }

    private Vector3 GetVelocity() {
        return rigidbody.velocity;
    }

}
