using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int totalScore;

    public List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private Score uiScore;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        totalScore = 0;
        Time.timeScale = 1;
    }

    public void DestroyGameManager()
    {
        Destroy(this.gameObject);
    }

    public void increaseScore(int scoreIncrease)
    {
        totalScore += scoreIncrease;

        uiScore.increaseScore(totalScore);
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void ReEngageEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyAI>().playerObject = GameObject.FindWithTag("Player");
            enemy.GetComponent<EnemyAI>().player = enemy.GetComponent<EnemyAI>().playerObject.transform;
        }

    }

    public void ObscurePlayer()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyAI>().playerObject = null;
            enemy.GetComponent<EnemyAI>().player = null;
        }
    }
}

