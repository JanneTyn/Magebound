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
    bool exploded = false;
    float boltAliveTime = 0;
    public float explosionSize = 0;
    public float groundDamage = 5;
    public bool explosionFin = false;
    public bool groundSet = false;
    Vector3 projectileDir;
    Vector3 playerLocation;
    private bool directionSet;
    private float dmgTimer = 0;


    // Update is called once per frame
    void Update()
    {

        if (directionSet && !collided)
        {
            var step = GetBoltSpeed() * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, projectileDir, step);
            boltAliveTime += Time.deltaTime;
            Debug.Log("projectileDir:" + projectileDir);
        }

        float dist = Vector3.Distance(transform.position, playerLocation);
        if (dist > 2000) { StartCoroutine(InitializeBurningGround()); collided = true; }
        else if (transform.position == projectileDir) { SetExplosionArea(); GetComponent<VisualEffect>().SendEvent("OnExplode"); collided = true; StartCoroutine(InitializeBurningGround()); }

        if (explosionFin)
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
        if (other.CompareTag("Enemy") && !exploded)
        {
            if (GetIsExplosive())
            {
                SetExplosionArea();
                Collider[] enemies = Physics.OverlapSphere(transform.position, explosionSize, GetExplosionLayer());

                foreach (Collider enemy in enemies)
                {
                    if (enemy.TryGetComponent<DamageSystem>(out DamageSystem dmg))
                    {
                        if (enemy.tag == "Enemy") dmg.CalculateDamage(GetDamage(), GetElementID());
                        if (GetManaRecoveryAmmount() > 0.1f)
                        {
                            GetManaSystem().GainMana(GetManaRecoveryAmmount());
                        }
                        else
                        {
                            GetManaSystem().GainMana();
                        }
                    }
                    else
                    {
                        Debug.Log("Damagesystem not found");
                    }
                }
                collided = true;
                GetComponent<VisualEffect>().SendEvent("OnExplode");
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

    private void OnTriggerStay(Collider other)
    {
        if (GetElementID() == 1)
        {
            dmgTimer += Time.deltaTime;
            if (dmgTimer >= 0.5f)
            {
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<DamageSystem>().CalculateDamage(groundDamage, true, GetStatusID(), GetStatusDuration(), GetElementID());
                }
                dmgTimer = 0;
            }
        }
    }

    IEnumerator InitializeBurningGround()
    {
        exploded = true;
        float t = 0;
        float life = GetComponent<VisualEffect>().GetFloat("GroundLife") - 1;
        while (t < life)
        {
            t += Time.deltaTime;
            yield return null;
        }
        explosionFin = true;
    }

    void SetExplosionArea()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        float timeMult = boltAliveTime * GetComponent<VisualEffect>().GetFloat("GrowSpeed");
        if (timeMult > 1) timeMult = 1;
        explosionSize = Mathf.Lerp(GetComponent<VisualEffect>().GetFloat("ExplosionSizeMin") + 1, GetComponent<VisualEffect>().GetFloat("ExplosionSizeMax"), timeMult);
        Debug.Log("Explosionsize: " + explosionSize);

        GetComponent<BoxCollider>().size = new Vector3(explosionSize, explosionSize, explosionSize);
        GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
    }
    
}
