using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text textField;
    private float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        elapsedTime += Time.deltaTime;

        UpdateElapsedTimeDisplay(elapsedTime);
    }

    void UpdateElapsedTimeDisplay(float timeToDisplay)
    {
        // Format time as minutes and seconds
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the TMP text
        textField.text = $"{minutes:00}:{seconds:00}";
    }
}
