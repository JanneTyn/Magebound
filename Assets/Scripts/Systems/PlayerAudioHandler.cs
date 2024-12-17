using System.Collections;
using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource[] basicAttack;
    [SerializeField] AudioSource explosionAttack;
    [SerializeField] AudioSource wallAndCrystalShardAttack;
    [SerializeField] AudioSource dash;
    [SerializeField] AudioSource notEnoughMana;

    private AudioSource selectedBasicAttack;
    private bool basicAttackIsPlaying = false;

    private void Start()
    {
        selectedBasicAttack = basicAttack[0];
    }

    public void PlayBasicAttack()
    {
        if (AudioManager.Instance != null)
        {
            if (!AudioManager.Instance.GamePaused)
            {
                if (!basicAttackIsPlaying)
                {
                    foreach (AudioSource audio in basicAttack)
                    {
                        AudioVolume(audio);
                    }

                    StartCoroutine(BasicAttackIsPlaying());
                }
            }
        }
    }

    public void PlayExplosion()
    {
        if (AudioManager.Instance != null)
        {
            if (!AudioManager.Instance.GamePaused)
            {
                AudioVolume(explosionAttack);
                explosionAttack.Play();
            }
        }

    }

    public void PlayWall()
    {
        if (AudioManager.Instance != null)
        {
            if (!AudioManager.Instance.GamePaused)
            {
                AudioVolume(wallAndCrystalShardAttack);
                wallAndCrystalShardAttack.Play();
            }
        }
    }

    public void PlayCrystalShard()
    {
        if (AudioManager.Instance != null)
        {
            if (!AudioManager.Instance.GamePaused)
            {
                AudioVolume(wallAndCrystalShardAttack);
                wallAndCrystalShardAttack.Play();
            }
        }
    }  

    public void PlayNotEnoughMana()
    {
        if (AudioManager.Instance != null)
        {
            if (!AudioManager.Instance.GamePaused)
            {
                AudioVolume(notEnoughMana);
                notEnoughMana.Play();
            }
        }
    }

    public void PlayDash()
    {
        if (AudioManager.Instance != null)
        {
            if (!AudioManager.Instance.GamePaused)
            {
                AudioVolume(dash);
                dash.Play();
            }
        }
    }

    IEnumerator BasicAttackIsPlaying()
    {
        selectedBasicAttack.Play();
        basicAttackIsPlaying = true;

        while (selectedBasicAttack.isPlaying)
        {
            yield return null;
        }

        selectedBasicAttack = basicAttack[Random.Range(0, basicAttack.Length)];
        basicAttackIsPlaying = false;
    }
    private void AudioVolume(AudioSource audioSource)
    {
        //Very inefficent way to do this...
        
        audioSource.volume = AudioManager.Instance.playerVolume;
    }
}
