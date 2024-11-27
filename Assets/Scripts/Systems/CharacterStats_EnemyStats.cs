using UnityEngine;

public class CharacterStats_EnemyStats : CharacterStats
{
    public override void ApplyDamage(float damage)
    {
        SetCurrentHealth(GetCurrentHealth() - damage);

        if (GetCurrentHealth() <= 0)
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        Destroy(gameObject);
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }

        Debug.Log("Enemy died!");
    }
}
