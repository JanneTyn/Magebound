using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellEffect_Projectile_BasicAttack : SpellEffect_Projectile
{
    SpellEffect otherSpellEffect;
    public override void CheckOverlap(int otherID)
    {
        foreach (int spellID in overlappableEffectID)
        {
            if (otherID == spellID)
            {
                Activate(otherID);

                break;
            }
        }
    }

    public override void Activate(int spellID)
    {
        //if needed
        //otherSpellEffect.Activate(spellID);

        //And what this one does
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (GetIsExplosive())
            {
                Collider[] enemies = Physics.OverlapSphere(transform.position, GetExplosionRadius(), GetExplosionLayer());

                foreach (Collider enemy in enemies)
                {
                    enemy.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetGiveStatus(), GetStatusID(), GetElementID());
                }
            } else
            {
                other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetGiveStatus(), GetStatusID(), GetElementID());
            }
        }
        else if (other.CompareTag("SpellEffect"))
        {
            otherSpellEffect = other.GetComponent<SpellEffect>();
            CheckOverlap(other.GetComponent<SpellEffect>().GetSpellID());
        }
    }

}
