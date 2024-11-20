using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnDelay = 1f; //Wait for the initial spawn delay
    private float spawnInterval = 2.5f;
    private int enemiesSpawned = 0;
    private List<Vector3> spawnPoints = new List<Vector3>
    {
        new Vector3(-3.057574f, 0.547f, 13.83197f),
        new Vector3(-8.142998f, 1f, -8.4376f),
        new Vector3(4.571501f, 1f, -1.891045f),
        new Vector3(12.4353f, 0.9999998f, -0.8511049f),
        new Vector3(15.0247f, 1f, 7.231292f)
    };
    private int lastSpawnIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        //Wait for the initial spawn delay
        yield return new WaitForSeconds(spawnDelay);

        //Endless enemy spawns
        while (true)
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

            Vector3 spawnLocation = spawnPoints[spawnIndex];
            Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
            enemiesSpawned++;
            lastSpawnIndex = spawnIndex;
        }
        else
        {
            Debug.LogWarning("EnemySpawner.cs: Enemy prefab or spawn points are not assigned.");
        }
    }
}
