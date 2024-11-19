using UnityEngine;

public class CharacterStats_PlayerStats : CharacterStats
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
        throw new System.NotImplementedException();
    }
}
