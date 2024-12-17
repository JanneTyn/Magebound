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
        if (!basicAttackIsPlaying)
        {
            foreach (AudioSource audio in basicAttack)
            {
                AudioVolume(audio);
            }

            StartCoroutine(BasicAttackIsPlaying());
        }

    }

    public void PlayExplosion()
    {
        AudioVolume(explosionAttack);
        explosionAttack.Play();
    }

    public void PlayWall()
    {
        AudioVolume(wallAndCrystalShardAttack);
        wallAndCrystalShardAttack.Play();
    }

    public void PlayCrystalShard()
    {
        AudioVolume(wallAndCrystalShardAttack);
        wallAndCrystalShardAttack.Play();
    }  

    public void PlayNotEnoughMana()
    {
        AudioVolume(notEnoughMana);
        notEnoughMana.Play();
    }

    public void PlayDash()
    {
        AudioVolume(dash);
        dash.Play();
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
        if(AudioManager.Instance != null)
        {
            audioSource.volume = AudioManager.Instance.playerVolume;
        }
    }
}
