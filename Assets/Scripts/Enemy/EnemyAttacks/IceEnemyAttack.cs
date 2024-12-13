using UnityEngine;

public class IceEnemyAttack : EnemyAttack
{
    protected override void HandlePlayerHit(Collider player)
    {
        player.GetComponent<DamageSystem>().CalculateDamage(damage, 2);
    }
}
