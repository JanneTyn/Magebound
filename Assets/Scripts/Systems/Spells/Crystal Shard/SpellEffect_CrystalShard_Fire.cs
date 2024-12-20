using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class SpellEffect_CrystalShard_Fire : SpellEffect_WorldEffect
{
    public float delay = 5;
    public float explosionSize = 5;
    float timer = 0;
    float finalDmg;
    bool delayPassed = false;
    bool explosionActive = false;
    SphereCollider areaEffect;
    MeshRenderer areaRender;
    VisualEffect vfx;
    [SerializeField] private Material red;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        areaEffect = GetComponentInChildren<SphereCollider>();
        areaRender = GetComponentInChildren<MeshRenderer>();
        vfx = GetComponent<VisualEffect>();
        SetManaSystem(GameObject.Find("Player").GetComponent<ManaSystem>());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay && !delayPassed) 
        { 
            delayPassed = true;
            Collider[] enemies = Physics.OverlapSphere(transform.position, explosionSize);

            foreach (Collider enemy in enemies)
            {
                if (enemy.TryGetComponent<DamageSystem>(out DamageSystem dmg))
                {
                    if (GetComponent<VisualEffect>().GetBool("Supercharged")) { finalDmg = GetDamage() * 2; Debug.Log("supercharge hit"); }
                    else finalDmg = GetDamage();

                    if (enemy.tag == "Enemy") dmg.CalculateDamage(finalDmg, GetElementID());

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
            StartCoroutine(DestroyAfterEffect());
        }
        else if (timer > delay - 0.2f && !explosionActive)
        {
            explosionActive = true;
            vfx.SetBool("IsExploding", true);
        }
    }

    IEnumerator DestroyAfterEffect()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
