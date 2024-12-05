using System.Linq.Expressions;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    CharacterStats characterStats;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    public void CalculateDamage(float damage, int elementID)
    {
        float finalDamage = calculateElementalDamage(damage, elementID);

        characterStats.ApplyDamage(finalDamage);
    }
    public void CalculateDamage(float damage, bool applyStatus, int statusID, float statusDuration, int elementID)
    {
        float finalDamage = calculateElementalDamage(damage, elementID);

        characterStats.ApplyDamage(finalDamage);

        if (applyStatus)
        {
            GetComponent<StatusManager>().Activate(statusID, statusDuration);
        }
    }
    public void CalculateDamage(float damage, bool applyStatus, int statusID, float statusDuration, float statusDamage, int elementID)
    {
        float finalDamage = calculateElementalDamage(damage, elementID);

        characterStats.ApplyDamage(finalDamage);

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
                finalDamage -= finalDamage * characterStats.GetFireResistance();
                break;

            //Electric
            case 03:
                finalDamage -= finalDamage * characterStats.GetFireResistance();
                break;

            default:
                Debug.Log("Error! Should not be here, apply element ID");
                break;
        }

        return finalDamage;
    }
}
