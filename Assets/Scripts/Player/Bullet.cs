using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    [SerializeField] private float force = 50f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] GameObject destroyEffect;

    private Pools pools;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 firingDirection;

    public void SetFiringDirection(Vector3 direction) {
        firingDirection = direction;
        AddForce();
    }

    private void Start() {
        pools = FindObjectOfType<Pools>();
    }

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void AddForce() {
        rigidbody.velocity = firingDirection.normalized * force;
    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            collision.gameObject.GetComponent<Enemy>().InstantiateBlood(collision.contacts[0].point);
            MoveToPool();
        } else if (!collision.gameObject.CompareTag("Obstacle")) {
            destroyEffect.GetComponent<ParticleSystem>().Play();
            MoveToPool();
        }
    }

    private void MoveToPool() {
        gameObject.SetActive(false);
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = initialRotation;
        rigidbody.velocity = Vector3.zero;
        pools.AddBulletToPool(gameObject);
    }
}
