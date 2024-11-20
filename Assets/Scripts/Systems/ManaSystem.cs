using System.Collections;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    CharacterStats characterStats;
    private float maxMana;
    private float currentMana;
    [SerializeField] private float passiveManaRegen;

    public float manaOnHitRecoverAmmount = 20;
    public ManaBar manaBar;

    IEnumerator manaRecovery;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();

        maxMana = characterStats.GetMaxMana();
        currentMana = maxMana;

        manaBar.SetMaxMana(maxMana); 
        manaBar.SetCurrentMana(currentMana);
    }

    public void UseMana(float manaAmmount) 
    { 
        currentMana -= manaAmmount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        if (manaRecovery != null)
        {
            StopCoroutine(manaRecovery);
        }
        manaRecovery = PassiveManaRecovery();
        StartCoroutine(manaRecovery);
    }

    public void GainMana()
    {
        currentMana += manaOnHitRecoverAmmount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        manaBar.SetCurrentMana(currentMana);
    }

    IEnumerator PassiveManaRecovery()
    {
        while (currentMana < maxMana)
        {
            currentMana += passiveManaRegen * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            manaBar?.SetCurrentMana(currentMana);

            yield return null;
        }

        manaRecovery = null;
    }
}
