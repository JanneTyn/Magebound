using UnityEngine;

public abstract class SpellEffect : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool giveStatus;
    [SerializeField] private int statusID;
    [SerializeField] private int spellID;
    [SerializeField] private int ElementID;


    public int[] overlappableEffectID;

    public virtual bool CheckOverlap(int otherID)
    {

        foreach (var id in overlappableEffectID)
        {
            if (id == otherID)
            {
                Debug.Log(spellID + " CheckOverlap = true");
                return true;         
            }else
            {
                Debug.Log(spellID + " CheckOverlap = false");
            }
        }

        Debug.Log(spellID + "Check overlap = false");
        return false;
    }

    public abstract void Activate(int spellID);

    public virtual int GetSpellID() { return spellID; }
    public virtual float GetDamage() { return damage; }
    public virtual bool GetGiveStatus() { return giveStatus; }
    public virtual int GetStatusID() { return statusID; }
    public virtual int GetElementID() { return ElementID; }

}
