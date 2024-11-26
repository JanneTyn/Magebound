using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnpointsObject;
    private float spawnDelay = 1f; //Wait for the initial spawn delay
    private float spawnInterval = 2.5f;
    private int enemiesSpawned = 0;
    private List<Transform> spawnPoints = new List<Transform>();
    private int lastSpawnIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform spawnpoint in spawnpointsObject.transform)
        {
            spawnPoints.Add(spawnpoint);
        }

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        //Wait for the initial spawn delay
        yield return new WaitForSeconds(spawnDelay);

        //Endless enemy spawns
        //while (true)
        while (enemiesSpawned < 1)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && spawnPoints.Count > 0)
        {
            int spawnIndex;

            //Make sure that next spawn point isn't same as last one
            do
            {
                spawnIndex = Random.Range(0, spawnPoints.Count); //Random spawn point
            } while (spawnIndex == lastSpawnIndex); //Repeat if same as last one

            Transform spawnLocation = spawnPoints[spawnIndex];
            Instantiate(enemyPrefab, spawnLocation.position, Quaternion.identity);
            enemiesSpawned++;
            lastSpawnIndex = spawnIndex;
        }
        else
        {
            Debug.LogWarning("EnemySpawner.cs: Enemy prefab or spawn points are not assigned.");
        }
    }
}
