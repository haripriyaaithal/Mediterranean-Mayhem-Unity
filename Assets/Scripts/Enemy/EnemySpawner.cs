using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [Header("Pool")]
    [SerializeField] private Pools pools;

    [Header("Enemy Parameters")]
    [SerializeField] private int enemyCount = 5;
    [SerializeField] private float spawnRadius = 6f;
    [SerializeField] private float spawnDelay = 3f;
    [SerializeField] UIManager uIManager;

    private int currentNumberOfEnemies = 0;
    private WaitForSeconds waitForSeconds;

    private int enemiesKilled = 0;

    private void Start() {
        waitForSeconds = new WaitForSeconds(spawnDelay);
    }

    public int GetEnemyCount() {
        return enemyCount;
    }

    private Vector3 GetRandomPosition() {
        Vector2 randomPosition = Random.insideUnitCircle.normalized * spawnRadius;
        return new Vector3(randomPosition.x, transform.position.y, randomPosition.y);
    }

    // Update is called once per frame
    void Update() {
        if (currentNumberOfEnemies < enemyCount) {
                StartCoroutine(InstantiateEnemyFromPool());
        }
    }

    private IEnumerator InstantiateEnemyFromPool() {
        yield return waitForSeconds;
        if (pools.GetEnemyPoolSize() > 0) {
            GameObject enemy = pools.GetEnemy();
            enemy.SetActive(true);
            enemy.transform.position = GetRandomPosition();
            IncreaseEnemyCount();
        }

    }


    private void DecreaseEnemyCount() {
        currentNumberOfEnemies--;
    }

    public void IncreaseEnemyCount() {
        currentNumberOfEnemies++;
    }

    public void AddToQueue(GameObject enemy) {
        pools.AddEnemyToPool(enemy);
        DecreaseEnemyCount();
        enemiesKilled++;
        uIManager.UpdateKills(enemiesKilled);
    }
}
