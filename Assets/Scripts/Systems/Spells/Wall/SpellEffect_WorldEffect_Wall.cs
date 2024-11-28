using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class SpellEffect_WorldEffect_Wall : SpellEffect_WorldEffect
{
    private GameObject parentObject;
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
            other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetElementID());
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
