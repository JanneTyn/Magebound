using Unity.VisualScripting;
using UnityEngine;

public class PlayerTestingGarbage : MonoBehaviour
{
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            DamageSystem damageSystem = GetComponent<DamageSystem>();

            damageSystem.CalculateDamage(100f, false, 0, 01);
        }

        if(Input.GetKeyDown(KeyCode.P)) 
        {
            LevelManager levelManager = GetComponent<LevelManager>();

            levelManager.GainExperience(20);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ManaSystem manaSystem = GetComponent<ManaSystem>();

            manaSystem.UseMana(100);
        }
    }

}
