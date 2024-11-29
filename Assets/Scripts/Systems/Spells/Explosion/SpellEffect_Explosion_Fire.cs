using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SpellEffect_Explosion_Fire : SpellEffect_Explosive
{
    float time = 0;
    float explosionDamageDelayTime = 0.3f;
    public float explosionEffectTime = 1f;
    bool dmgActive = false;
    bool collided = false;
    public bool explosionFin = false;
    public bool groundSet = false;
    Vector3 projectileDir;
    Vector3 playerLocation;
    private bool directionSet;

    // Update is called once per frame
    void Update()
    {

        if (directionSet && !collided)
        {
            var step = GetBoltSpeed() * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, projectileDir, step);
            Debug.Log("projectileDir:" + projectileDir);
        }

        float dist = Vector3.Distance(transform.position, playerLocation);
        if (dist > 2000) { StartCoroutine(InitializeBurningGround()); }
        else if (transform.position == projectileDir) { GetComponent<VisualEffect>().SetBool("IsExploding", true); StartCoroutine(InitializeBurningGround()); }

        if (explosionFin && groundSet)
        {        
            Destroy(gameObject);
        }
    }

    public void SetProjectileDirection(Vector3 dir, Vector3 playerLoc)
    {
        projectileDir = dir;
        transform.LookAt(projectileDir);
        directionSet = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (GetIsExplosive())
            {
                Collider[] enemies = Physics.OverlapSphere(transform.position, GetExplosionRadius(), GetExplosionLayer());

                foreach (Collider enemy in enemies)
                {
                    if (enemy.TryGetComponent<DamageSystem>(out DamageSystem dmg)) 
                    {
                        dmg.CalculateDamage(GetDamage(), GetElementID());
                    }
                    else
                    {
                        Debug.Log("Damagesystem not found");
                    }
                }
                collided = true;             
                GetComponent<VisualEffect>().SetBool("IsExploding", true);
                StartCoroutine(InitializeBurningGround());
            }
            else
            {
                other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetElementID());
                Destroy(gameObject);
            }
        }
        /* else if (other.CompareTag("SpellEffect"))
        {
            otherSpellEffect = other.GetComponent<SpellEffect>();
            CheckOverlap(other.GetComponent<SpellEffect>().GetSpellID());
        } */
    }

    IEnumerator InitializeBurningGround()
    {
        float t = 0;

        while (t < explosionEffectTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        explosionFin = true;
    }
}
