using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private float fadeInDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        myUIGroup = GetComponent<CanvasGroup>();

        StartCoroutine(FadeIn(fadeInDuration));

    }

    public void SetUI(bool set)
    {
        this.gameObject.SetActive(set);
    }

    IEnumerator FadeIn(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            myUIGroup.alpha = elapsedTime / duration;

            yield return null;
        }

        Time.timeScale = 0;
    }

}
