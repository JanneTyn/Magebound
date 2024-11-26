using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    public GameObject iceEnemyPrefab;
    public GameObject fireEnemyPrefab;
    public GameObject electricEnemyPrefab;
    public GameObject spawnpointsObject;

    private float spawnDelay = 1f; //Initial spawn delay
    private float spawnInterval = 2.5f;
    private int enemiesSpawned = 0;
    private List<Transform> spawnPoints = new List<Transform>();
    private int lastSpawnIndex = -1;
    private GameObject lastSpawnedPrefab = null;

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
        while (enemiesSpawned < 10)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (iceEnemyPrefab != null && fireEnemyPrefab != null && electricEnemyPrefab != null && spawnPoints.Count > 0)
        {
            int spawnIndex;

            //Make sure that next spawn point isn't same as last one
            do
            {
                spawnIndex = Random.Range(0, spawnPoints.Count); //Random spawn point
            } while (spawnIndex == lastSpawnIndex); //Repeat if same as last one

            Transform spawnLocation = spawnPoints[spawnIndex];
            lastSpawnIndex = spawnIndex;

            //Randomly select an enemy prefab and make sure it's not same as last one.
            GameObject[] enemyPrefabs = {iceEnemyPrefab, fireEnemyPrefab, electricEnemyPrefab};
            GameObject selectedPrefab;

            do
            {
                selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            } while (selectedPrefab == lastSpawnedPrefab);

            //Spawn the selected enemy prefab
            //Instantiate(selectedPrefab, spawnLocation.position, Quaternion.identity);
            Instantiate(electricEnemyPrefab, spawnLocation.position, Quaternion.identity);
            lastSpawnedPrefab = selectedPrefab;
            enemiesSpawned++;
            
        }
        else
        {
            Debug.LogWarning("EnemySpawner.cs: Enemy prefab or spawn points are not assigned.");
        }
    }
}
