using System.Linq.Expressions;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    CharacterStats characterStats;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }
    public void CalculateDamage(float damage, bool applyStatus, int statusID, int ElementID)
    {
        float finalDamage = damage;

        switch (ElementID)
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
        

        characterStats.ApplyDamage(finalDamage);

        if(applyStatus)
        {
            GetComponent<StatusManager>().Activate(statusID);
        }
    }
}
