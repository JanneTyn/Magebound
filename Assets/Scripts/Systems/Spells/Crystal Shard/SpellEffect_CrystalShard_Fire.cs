using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpellEffect_CrystalShard_Fire : SpellEffect_WorldEffect
{
    public float delay = 5;
    public float explosionSize = 5;
    float timer = 0;
    bool delayPassed = false;
    SphereCollider areaEffect;
    MeshRenderer areaRender;
    [SerializeField] private Material red;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        areaEffect = GetComponentInChildren<SphereCollider>();
        areaRender = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay && !delayPassed) 
        { 
            delayPassed = true;
            areaEffect.enabled = true;
            areaRender.material = red;
            Collider[] enemies = Physics.OverlapSphere(transform.position, explosionSize);

            foreach (Collider enemy in enemies)
            {
                if (enemy.TryGetComponent<DamageSystem>(out DamageSystem dmg))
                {
                    if (enemy.tag == "Enemy") dmg.CalculateDamage(GetDamage(), GetElementID());
                }
                else
                {
                    Debug.Log("Damagesystem not found");
                }
            }
            StartCoroutine(DestroyAfterEffect());
        }
    }

    IEnumerator DestroyAfterEffect()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
