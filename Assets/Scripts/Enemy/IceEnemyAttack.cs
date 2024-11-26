using UnityEngine;

public class IceEnemyAttack : EnemyAttack
{
    protected override void HandlePlayerHit(Collider player)
    {
        player.GetComponent<DamageSystem>().CalculateDamage(50f, false, 0, 2);
        Debug.Log("Ice enemy hit player!");
    }
}
