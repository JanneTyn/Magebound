using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] float maxMana;
    [SerializeField] float fireResistance;
    [SerializeField] float iceResistance;
    [SerializeField] float electricResistance;
    [SerializeField] int currentElement;

    public virtual float GetMaxHealth() { return maxHealth; }
    public virtual float GetCurrentHealth() {  return currentHealth; }
    public virtual float GetMaxMana() {  return maxMana; }
    public virtual float GetFireResistance() {  return fireResistance; }
    public virtual float GetIceResistance() { return iceResistance; }
    public virtual float GetElectricResistance() { return electricResistance; }
    public virtual int GetCurrentElement() { return currentElement; }

    public virtual void SetMaxHealth(float newMaxHealth) { maxHealth = newMaxHealth; }
    public virtual void SetCurrentHealth (float newCurrentHealth) {  currentHealth = newCurrentHealth; }
    public virtual void SetMaxMana (float newMaxMana) {  maxMana = newMaxMana; }
    public virtual void SetFireResistance(float newFireResistance) { fireResistance = newFireResistance; }
    public virtual void SetIceResistance(float newIceResistance) { iceResistance = newIceResistance; }
    public virtual void SetElectricResistance(float newElectricResistance) { electricResistance = newElectricResistance; } 
    public virtual void SetCurrentElement(int newElement) { currentElement = newElement; } 

    public abstract void ApplyDamage(float damage);


}
