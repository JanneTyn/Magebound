using System.Linq.Expressions;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    CharacterStats characterStats;
    [SerializeField] AudioClip[] takeDamageAudioClip;
    AudioSource audioSource;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void CalculateDamage(float damage, int elementID)
    {
        float finalDamage = calculateElementalDamage(damage, elementID);

        characterStats.ApplyDamage(finalDamage);
        PlayTakeDamageAudio();
    }
    public void CalculateDamage(float damage, bool applyStatus, int statusID, float statusDuration, int elementID)
    {
        float finalDamage = calculateElementalDamage(damage, elementID);

        characterStats.ApplyDamage(finalDamage);
        PlayTakeDamageAudio();

        if (applyStatus)
        {
            GetComponent<StatusManager>().Activate(statusID, statusDuration);
        }
    }
    public void CalculateDamage(float damage, bool applyStatus, int statusID, float statusDuration, float statusDamage, int elementID)
    {
        float finalDamage = calculateElementalDamage(damage, elementID);

        characterStats.ApplyDamage(finalDamage);
        PlayTakeDamageAudio();

        if (applyStatus)
        {
            GetComponent<StatusManager>().Activate(statusID, statusDuration, statusDamage);
        }
    }


    private float calculateElementalDamage(float damage, int elementID)
    {
        float finalDamage = damage;

        switch (elementID)
        {
            //fire
            case 01:
                finalDamage -= finalDamage * characterStats.GetFireResistance();
                break;

            // Ice
            case 02:
                finalDamage -= finalDamage * characterStats.GetIceResistance();
                break;

            //Electric
            case 03:
                finalDamage -= finalDamage * characterStats.GetElectricResistance();
                break;

            default:
                Debug.Log("Error! Should not be here, apply element ID");
                break;
        }

        return finalDamage;
    }

    private void PlayTakeDamageAudio()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (!GetComponent<CharacterStats_PlayerStats>().playerDead)
            {
                if (takeDamageAudioClip.Length != 0)
                {
                    audioSource.clip = takeDamageAudioClip[Random.Range(0, takeDamageAudioClip.Length)];
                    audioSource.Play();
                }
            }
        }
        else if (takeDamageAudioClip.Length != 0)
        {
            audioSource.clip = takeDamageAudioClip[Random.Range(0, takeDamageAudioClip.Length)];
            audioSource.Play();
        }
    }


}
