using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class SpellEffect_Explosion_Thunder : SpellEffect_Explosive
{

    public bool explosionFin = false;
    float explosionDamageDelayTime = 0.3f;
    public float explosionEffectTime = 1f;
    bool collided = false;
    bool enemyHit = false;
    bool shockwaveEnabled = false;
    bool colliderChanged = false;
    float boltAliveTime = 0;
    float explosionSize = 0;
    Vector3 projectileDir;
    Vector3 playerLocation;
    private bool directionSet;
    [SerializeField] private AudioSource explosionImpactSound;
    private bool impactSoundPlayed = false;

    // Update is called once per frame
    void Update()
    {
        if (directionSet && !collided)
        {
            var step = GetBoltSpeed() * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, projectileDir, step);
            Debug.Log("projectileDir:" + projectileDir);
            boltAliveTime += Time.deltaTime;
        }

        float dist = Vector3.Distance(transform.position, playerLocation);
        if (dist > 2000) { StartCoroutine(InitializeThunderShockwave()); collided = true; }
        else if (transform.position == projectileDir) { GetComponent<VisualEffect>().SendEvent("OnExplode"); collided = true; StartCoroutine(InitializeThunderShockwave()); }
        
    }

    public void SetProjectileDirection(Vector3 dir, Vector3 playerLoc)
    {
        projectileDir = dir;
        transform.LookAt(projectileDir);
        directionSet = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemyHit)
        {
            if (GetIsExplosive())
            {
                if (!shockwaveEnabled) SetExplosionArea();
                Collider[] enemies = Physics.OverlapSphere(transform.position, explosionSize, GetExplosionLayer());

                foreach (Collider enemy in enemies)
                {
                    if (enemy.TryGetComponent<DamageSystem>(out DamageSystem dmg))
                    {
                        if (enemy.tag == "Enemy") dmg.CalculateDamage(GetDamage(), shockwaveEnabled, GetStatusID(), GetStatusDuration(), GetElementID());
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

                if (!shockwaveEnabled) StartCoroutine(InitializeThunderShockwave());
                enemyHit = true;
            }
            else
            {
                other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), shockwaveEnabled, GetStatusID(), GetStatusDuration(), GetElementID());
                Destroy(gameObject);
            }
        }
        /* else if (other.CompareTag("SpellEffect"))
        {
            otherSpellEffect = other.GetComponent<SpellEffect>();
            CheckOverlap(other.GetComponent<SpellEffect>().GetSpellID());
        } */
    }

    IEnumerator InitializeThunderShockwave()
    {
        if(!impactSoundPlayed)
        {
            if (AudioManager.Instance != null)
            {
                explosionImpactSound.volume = AudioManager.Instance.effectVolume;
                explosionImpactSound.Play();
            }
            impactSoundPlayed = true;
        }

        float t = 0;
        float shockwaveTime = GetComponent<VisualEffect>().GetFloat("ShockwaveDuration");
        while (t < explosionDamageDelayTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        GetComponent<VisualEffect>().SetBool("IsShockwaving", true);
        explosionSize = GetComponent<VisualEffect>().GetFloat("ShockwaveSize") / 2;
        shockwaveEnabled = true;
        enemyHit = false;
        while (t < shockwaveTime)
        {
            if (t > shockwaveTime / 3 && !colliderChanged)
            {
                colliderChanged = true;
                GetComponent<BoxCollider>().size = new Vector3(explosionSize, explosionSize, explosionSize);
            }
            t += Time.deltaTime;
            yield return null;
        }
        explosionFin = true;

        Destroy(gameObject);
    }

    void SetExplosionArea()
    {
        float timeMult = boltAliveTime * GetComponent<VisualEffect>().GetFloat("GrowSpeed");
        if (timeMult > 1) timeMult = 1;
        explosionSize = Mathf.Lerp(GetComponent<VisualEffect>().GetFloat("ExplosionSizeMin"), GetComponent<VisualEffect>().GetFloat("ExplosionSizeMax"), timeMult);

        GetComponent<BoxCollider>().size = new Vector3(explosionSize, explosionSize, explosionSize);
        GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
    }
}
