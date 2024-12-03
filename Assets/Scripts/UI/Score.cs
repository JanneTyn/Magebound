using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{

    [SerializeField] TMP_Text scoreText;

    public void increaseScore(int totalScore)
    {
        scoreText.text = totalScore.ToString();
    }
}
