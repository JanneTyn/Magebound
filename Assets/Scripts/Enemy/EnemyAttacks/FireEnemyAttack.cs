using UnityEngine;

public class FireEnemyAttack : EnemyAttack
{
    protected override void HandlePlayerHit(Collider player)
    {
        player.GetComponent<DamageSystem>().CalculateDamage(damage, 1);
        Debug.Log("Fire enemy hit player!");
    }
}
