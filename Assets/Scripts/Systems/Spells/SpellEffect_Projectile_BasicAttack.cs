using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellEffect_Projectile_BasicAttack : SpellEffect_Projectile
{
    SpellEffect otherSpellEffect;
    Vector3 projectileDir;
    Vector3 playerLocation;
    private bool directionSet;
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

    void Update()
    {
        if (directionSet)
        {
            var step = GetProjectileSpeed() * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, projectileDir, step);
            Debug.Log("projectileDir:" + projectileDir);
        }

        float dist = Vector3.Distance(transform.position, playerLocation);
        if (dist > 20) { Destroy(gameObject); }
    }


    public void SetProjectileDirection(Vector3 dir, Vector3 playerLoc)
    {
        Vector3 direction = dir - playerLoc;
        playerLocation = playerLoc;
        projectileDir = dir + (direction * 1000);
        projectileDir.y = playerLoc.y;
        transform.LookAt(projectileDir);
        directionSet = true;
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
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("SpellEffect"))
        {
            otherSpellEffect = other.GetComponent<SpellEffect>();
            CheckOverlap(other.GetComponent<SpellEffect>().GetSpellID());
        }
    }

}
