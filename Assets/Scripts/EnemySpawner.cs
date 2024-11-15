using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Vector3 spawnLocation = new Vector3(-3.057574f, 0.547f, 13.83197f);
    public float spawnDelay = 3f;
    public float spawnInterval = 5f;
    private int enemiesSpawned = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        //Wait for the initial spawn delay
        yield return new WaitForSeconds(spawnDelay);

        //while (true)
        while (enemiesSpawned < 10)
        {
            SpawnEnemy();

            //Wait for the next interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            //Spawn the enemy at the spawn location
            Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
            enemiesSpawned++;
            
            if (enemiesSpawned == 1)
            {
                Debug.Log($"{enemiesSpawned} enemy spawned.");
            }
            else
            {
                Debug.Log($"{enemiesSpawned} enemies spawned.");
            }
            //Debug.LogWarning("Enemy prefab is assigned.");
        }
        else
        {
            Debug.LogWarning("Enemy prefab is not assigned.");
        }
    }
}
