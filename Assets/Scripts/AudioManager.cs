using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource mainAudio;
    [SerializeField] private AudioSource fireAudio;
    [SerializeField] private AudioSource iceAudio;
    [SerializeField] private AudioSource electricAudio;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void SwitchAudio(int id, float fadeTime)
    {
        switch(id)
        {
            case 1:
                StartCoroutine(FadeOut(iceAudio, fadeTime));
                StartCoroutine(FadeOut(electricAudio, fadeTime));
                StartCoroutine(FadeIn(fireAudio, fadeTime));
                break;
            case 2:
                StartCoroutine(FadeOut(fireAudio, fadeTime));
                StartCoroutine(FadeOut(electricAudio, fadeTime));
                StartCoroutine(FadeIn(iceAudio, fadeTime));
                break;
            case 3:
                StartCoroutine(FadeOut(fireAudio, fadeTime));
                StartCoroutine(FadeOut(iceAudio, fadeTime));
                StartCoroutine(FadeIn(electricAudio, fadeTime));
                break;
        }
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    private IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        //yield return new WaitForSeconds(fadeTime); //Maybe need this if gets called too fast

        audioSource.volume = 0f;
        while (audioSource.volume < mainAudio.volume)
        {
            audioSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}
