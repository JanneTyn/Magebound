using System.Collections;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    CharacterStats characterStats;
    [SerializeField] private float maxMana;
    [SerializeField] private float currentMana;
    [SerializeField] private float passiveManaRegen;

    public float manaOnHitRecoverAmmount = 20;
    public ManaBar manaBar; //UI Manabar

    IEnumerator manaRecovery;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();

        //Set original Mana values
        maxMana = characterStats.GetMaxMana();
        currentMana = maxMana;

        //Set Mana values to UI
        manaBar.SetMaxMana(maxMana); 
        manaBar.SetCurrentMana(currentMana);
    }

    public float GetMana() { return currentMana; }

    public void UseMana(float manaAmmount) 
    { 
        currentMana -= manaAmmount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        //Start passive mana recover after using Mana
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

        //Update UI
        manaBar.SetCurrentMana(currentMana);
    }

    public void GainMana(float manaRecoveryAmmount)
    {
        currentMana += manaRecoveryAmmount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        //Update UI
        manaBar.SetCurrentMana(currentMana);
    }

    public void LevelUp(float maxMana)
    {
        this.maxMana += maxMana;
        currentMana = this.maxMana;

        manaBar.SetMaxMana(this.maxMana);
        manaBar.SetCurrentMana(this.maxMana);
    }

    IEnumerator PassiveManaRecovery()
    {
        while (currentMana < maxMana)
        {
            currentMana += passiveManaRegen * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);

            //Update UI
            manaBar.SetCurrentMana(currentMana);

            yield return null;
        }

        manaRecovery = null;
    }

}
