using UnityEngine;

public class SpellEffect_WorldEffect_SmokeCloud : SpellEffect_WorldEffect
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
        foreach(Collider enemy in enemies)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
