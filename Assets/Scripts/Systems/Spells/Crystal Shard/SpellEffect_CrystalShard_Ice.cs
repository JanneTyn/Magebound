using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpellEffect_CrystalShard_Ice : SpellEffect_WorldEffect
{
    public float shardDuration = 3;
    public float explosionSize = 5;
    float timer = 0;
    float secs = 0;
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
        SetManaSystem(GameObject.Find("Player").GetComponent<ManaSystem>());
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
                        if (enemy.tag == "Enemy") dmg.CalculateDamage(GetDamage(), true, GetStatusID(), 0.45f, GetElementID());
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
                secs += 0.5f;
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
