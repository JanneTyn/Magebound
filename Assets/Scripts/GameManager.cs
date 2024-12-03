using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int totalScore;

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
        DontDestroyOnLoad(gameObject); // Persist through scenes
    }

    public void increaseScore(int scoreIncrease)
    {
        totalScore += scoreIncrease;

        uiScore.increaseScore(totalScore);
    }

}
