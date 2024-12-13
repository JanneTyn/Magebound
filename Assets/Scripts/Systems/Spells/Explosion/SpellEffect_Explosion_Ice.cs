using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SpellEffect_Explosion_Ice : SpellEffect_Explosive
{
    public bool explosionFin = false;
    float explosionDamageDelayTime = 0.3f;
    public float explosionEffectTime = 1f;
    bool collided = false;
    bool enemyHit = false;
    float boltAliveTime = 0;
    float explosionSize = 0;
    Vector3 projectileDir;
    Vector3 playerLocation;
    private bool directionSet;
    [SerializeField] private AudioSource explosionImpactSound;


    // Update is called once per frame
    void Update()
    {
        if (directionSet && !collided)
        {
            var step = GetBoltSpeed() * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, projectileDir, step);
            //Debug.Log("projectileDir:" + projectileDir);
            boltAliveTime += Time.deltaTime;
        }

        float dist = Vector3.Distance(transform.position, playerLocation);
        if (dist > 2000) { StartCoroutine(InitializeIcePillar()); collided = true; }
        else if (transform.position == projectileDir) { SetExplosionArea(); GetComponent<VisualEffect>().SendEvent("OnExplode"); collided = true; StartCoroutine(InitializeIcePillar()); }
    }

    public override void Activate(int spellID)
    {
        VisualEffect visualEffect = GetComponent<VisualEffect>();

        switch (spellID)
        {

            case 101: //overlap with fire basic attack

                GameObject pillarShard = Resources.Load<GameObject>("IceExplosionPillarShard");

                for (int i = 0; i < 361;)
                {
                    GameObject spawnedPillarShard = Instantiate(pillarShard, transform.position, Quaternion.Euler(0, Random.Range(i - 20, i- 20), 0));
                    
                    Rigidbody shardRb = spawnedPillarShard.GetComponent<Rigidbody>();

                    if(shardRb != null)
                    {
                        Vector3 flingDirection = spawnedPillarShard.transform.forward;
                        Vector3 arcDirection = flingDirection + Vector3.up * Random.Range(0.5f, 1.5f);

                        shardRb.AddForce(arcDirection * Random.Range(3f, 6f), ForceMode.Impulse);
                    }

                    i += 90;

                    
                }

                Destroy(gameObject);

                break;
            case 301: //overlap with electric basic attack

                GameObject pillarProjectile = Resources.Load<GameObject>("IceExplosionPillarProjectile");
                for (int i = 0; i < 361;)
                {
                    Instantiate(pillarProjectile, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(0, i, 0));
                    i += 45;
                }
                

                break;

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
        if (other.CompareTag("Enemy") && !enemyHit)
        {
            if (GetIsExplosive())
            {
                SetExplosionArea();
                Collider[] enemies = Physics.OverlapSphere(transform.position, explosionSize, GetExplosionLayer());

                foreach (Collider enemy in enemies)
                {
                    if (enemy.TryGetComponent<DamageSystem>(out DamageSystem dmg))
                    {
                        if (enemy.tag == "Enemy") dmg.CalculateDamage(GetDamage(), true, GetStatusID(), GetStatusDuration(), GetElementID());
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
                
                StartCoroutine(InitializeIcePillar());
                enemyHit = true;
            }
            else
            {
                other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), true, GetStatusID(), GetStatusDuration(), GetElementID());
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("SpellEffect"))
        {
           if(CheckOverlap(other.GetComponent<SpellEffect>().GetSpellID()))
           {
                //Currently nothing
           }
        }
    }

    IEnumerator InitializeIcePillar()
    {
        GetComponent<BoxCollider>().size = new Vector3(1, 20, 1);

        float t = 0;
        float pillarTime = GetComponent<VisualEffect>().GetFloat("SpikeDuration");
        while (t < pillarTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        explosionFin = true;
        Destroy(gameObject);
    }

    void SetExplosionArea()
    {
        explosionImpactSound.Play();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        float timeMult = boltAliveTime * GetComponent<VisualEffect>().GetFloat("GrowSpeed");
        if (timeMult > 1) timeMult = 1;
        explosionSize = Mathf.Lerp(GetComponent<VisualEffect>().GetFloat("ExplosionSizeMin"), GetComponent<VisualEffect>().GetFloat("ExplosionSizeMax"), timeMult);

        GetComponent<BoxCollider>().size = new Vector3(explosionSize, explosionSize, explosionSize);
        GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
    }
}
