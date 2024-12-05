using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class SpellEffect_WorldEffect_Wall : SpellEffect_WorldEffect
{
    private GameObject parentObject;
    public float statusDuration;
    public float length;
    public float size;

    private void Start()
    {
        parentObject = this.gameObject;
        SpawnWall(length, size);
    }

    public void SpawnWall(float length, float size)
    {
        BoxCollider collider = parentObject.GetComponent<BoxCollider>();

        collider.size = new Vector3 (length * 2, size , 1);

        VisualEffect visualEffect = parentObject.GetComponent<VisualEffect>();

        visualEffect.SetFloat("Width", length);
        visualEffect.SetFloat("SpikeScale", size);
        visualEffect.SetFloat("Duration", GetDuration());

        if (parentObject.GetComponent<NavMeshObstacle>() != null)
        {
            var obstacle = parentObject.GetComponent<NavMeshObstacle>();

            obstacle.size = new Vector3(length * 2, size, 1);
        }

        StartCoroutine(lifecycle());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (GetGiveStatus())
            {
                if(GetDamage() > 0.1f)
                {
                    other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetGiveStatus(), GetStatusID(), statusDuration, GetDamage(), GetElementID());
                    if(GetManaRecoveryAmmount() > 0.1f)
                    {
                        GetManaSystem().GainMana(GetManaRecoveryAmmount());
                    }
                    else
                    {
                        GetManaSystem().GainMana();
                    }
                    
                } else
                {
                    other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetGiveStatus(), GetStatusID(), statusDuration, GetElementID());
                    if (GetManaRecoveryAmmount() > 0.1f)
                    {
                        GetManaSystem().GainMana(GetManaRecoveryAmmount());
                    }
                    else
                    {
                        GetManaSystem().GainMana();
                    }
                }
            }
            else
            {
                other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetElementID());
                if (GetManaRecoveryAmmount() > 0.1f)
                {
                    GetManaSystem().GainMana(GetManaRecoveryAmmount());
                }
                else
                {
                    GetManaSystem().GainMana();
                }
            }

        } 
        else if (other.gameObject.CompareTag("SpellEffect"))
        {
            if (CheckOverlap(other.GetComponent<SpellEffect>().GetSpellID()))
            {
                other.GetComponent<SpellEffect>().Activate(GetSpellID());
            }
        }
    }

    private IEnumerator lifecycle()
    {
        yield return new WaitForSeconds(GetDuration());

        Destroy(this.gameObject);
    }


}
