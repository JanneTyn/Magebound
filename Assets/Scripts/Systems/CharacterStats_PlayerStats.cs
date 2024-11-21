using UnityEngine;

public class CharacterStats_PlayerStats : CharacterStats
{
    public HealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(GetMaxHealth());
        healthBar.SetCurrentHealth(GetCurrentHealth());
    }

    public override void ApplyDamage(float damage)
    {
        SetCurrentHealth(GetCurrentHealth() - damage);

        healthBar.SetCurrentHealth(GetCurrentHealth());

        if (GetCurrentHealth() <= 0)
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        throw new System.NotImplementedException();
    }
}
