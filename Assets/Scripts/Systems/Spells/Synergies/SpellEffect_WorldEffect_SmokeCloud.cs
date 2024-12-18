using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SpellEffect_WorldEffect_SmokeCloud : SpellEffect_WorldEffect
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Initialize(GetDuration()));

        Collider[] enemies = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
        foreach(Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetElementID());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ObscurePlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ReEngageEnemies();
        }
    }

    private IEnumerator Initialize(float duration)
    {
        if (GetComponent<VisualEffect>() != null)
        {
            VisualEffect visualEffect = GetComponent<VisualEffect>();
            visualEffect.SetFloat("Duration", duration);
        }


        yield return new WaitForSeconds(duration);
        GameManager.Instance.ReEngageEnemies();

        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}

