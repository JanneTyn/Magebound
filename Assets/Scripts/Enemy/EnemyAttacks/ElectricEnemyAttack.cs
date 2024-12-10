using UnityEngine;
using UnityEngine.UIElements;

public class ElectricEnemyAttack : EnemyAttack
{

    protected override void HandlePlayerHit(Collider player)
    {
        player.GetComponent<DamageSystem>().CalculateDamage(damage, 3);
        Debug.Log("Electric enemy hit player!");
    }

    public override void Attack()
    {
        if (GetComponent<Animator>().GetBool("IsDead")) return;
        base.Attack();
    }
}
