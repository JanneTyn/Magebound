using UnityEngine;

public abstract class SpellEffect : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool giveStatus;
    [SerializeField] private int statusID;
    [SerializeField] private int spellID;
    [SerializeField] private int ElementID;
    [SerializeField] private float projectileSpeed = 8;
    [SerializeField] private float travelDistance = 20;

    public int[] overlappableEffectID;

    public abstract void CheckOverlap(int otherID);
    public abstract void Activate(int spellID);

    public virtual int GetSpellID() { return spellID; }
    public virtual float GetDamage() { return damage; }
    public virtual bool GetGiveStatus() { return giveStatus; }
    public virtual int GetStatusID() { return statusID; }
    public virtual int GetElementID() { return ElementID; }
    public virtual float GetProjectileSpeed() { return projectileSpeed; }
    public virtual float GetTravelDistance() { return travelDistance; }

}
