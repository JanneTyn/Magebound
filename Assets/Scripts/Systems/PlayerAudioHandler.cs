using System.Collections;
using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource[] basicAttack;
    [SerializeField] AudioSource explosionAttack;

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
            StartCoroutine(BasicAttackIsPlaying());
        }

    }

    public void PlayExplosion()
    {
        if (!explosionAttack.isPlaying)
        {
            explosionAttack.Play();
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
}