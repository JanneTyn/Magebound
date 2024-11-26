using UnityEngine;

public class ElectricEnemyAttack : EnemyAttack
{
    protected override void HandlePlayerHit(Collider player)
    {
        player.GetComponent<DamageSystem>().CalculateDamage(50f, false, 0, 3);
        Debug.Log("Electric enemy hit player!");
    }
}
