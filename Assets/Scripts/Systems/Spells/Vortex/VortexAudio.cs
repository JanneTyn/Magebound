using UnityEngine;

public class VortexAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(AudioManager.Instance != null)
        {
            audioSource.volume = AudioManager.Instance.effectVolume;
            audioSource.Play();
        }
    }

}
