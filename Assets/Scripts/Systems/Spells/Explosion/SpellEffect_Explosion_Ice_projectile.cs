using UnityEngine;

public class SpellEffect_Explosion_Ice_projectile : MonoBehaviour
{
    public float speed = 10.0f;
    public float damage = 30.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GetComponent<DamageSystem>().CalculateDamage(damage, 3);
        }
    }
}
