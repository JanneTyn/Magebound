using System.Collections;
using UnityEngine;

public class SpellEffect_CrystalShard_Thunder : SpellEffect_WorldEffect
{
    public float shardDuration = 3;
    public float explosionSize = 5;
    float timer = 0;
    float secs = 1f;
    bool delayPassed = false;
    SphereCollider areaEffect;
    MeshRenderer areaRender;
    [SerializeField] private Material red;
    [SerializeField] private float statusDuration = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        areaEffect = GetComponentInChildren<SphereCollider>();
        areaRender = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < shardDuration)
        {
            delayPassed = true;
            //areaEffect.enabled = true;

            if (timer > secs)
            {
                Collider[] enemies = Physics.OverlapSphere(transform.position, explosionSize);

                foreach (Collider enemy in enemies)
                {
                    if (enemy.TryGetComponent<DamageSystem>(out DamageSystem dmg))
                    {
                        if (enemy.tag == "Enemy") dmg.CalculateDamage(GetDamage(), true, GetStatusID(), 1, GetElementID());
                    }
                    else
                    {
                        Debug.Log("Damagesystem not found");
                    }
                }
                secs++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }

    IEnumerator DestroyAfterEffect()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
