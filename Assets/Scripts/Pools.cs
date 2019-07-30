using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour {

    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bloodParticlesPreafab;

    [Header("Player")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int maxBullets = 20;

    private Queue<GameObject> enemyPool;
    private Queue<GameObject> bulletPool;

    private EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start() {
        enemyPool = new Queue<GameObject>();
        bulletPool = new Queue<GameObject>();

        enemySpawner = FindObjectOfType<EnemySpawner>();

        FillEnemyPool();
        FillBulletPool();
    }

    #region Enemy Pool
    private void FillEnemyPool() {
        for (int enemy = 0; enemy < enemySpawner.GetEnemyCount(); enemy++) {
            enemyPool.Enqueue(InstantiateEnemy());
        }
    }

    private GameObject InstantiateEnemy() {
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        enemy.SetActive(false);
        return enemy;
    }

    public GameObject GetEnemy() {
        return enemyPool.Dequeue();
    }

    public void AddEnemyToPool(GameObject enemy) {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }

    public int GetEnemyPoolSize() {
        return enemyPool.Count;
    }

    #endregion

    #region Bullet Pool
    private void FillBulletPool() {
        for (int bullets = 0; bullets < maxBullets; bullets++) {
            bulletPool.Enqueue(InstantiateBullet());
        }
    }

    private GameObject InstantiateBullet() {
        GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
        bullet.SetActive(false);
        return bullet;
    }

    public GameObject GetBullet() {
        return bulletPool.Dequeue();
    }

    public void AddBulletToPool(GameObject bullet) {
        bullet.SetActive(false);
        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;
        bulletPool.Enqueue(bullet);
    }
    #endregion
}