using UnityEngine;

public class IsPlayerInAttackRange : BTNode
{
    private Transform enemy;
    private Transform player;
    private float attackRange;

    public IsPlayerInAttackRange(Transform enemy, Transform player, float attackRange)
    {
        this.enemy = enemy;
        this.player = player;
        this.attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        if(player != null)
        {
            float distance = Vector3.Distance(enemy.position, player.position);
            if (distance <= attackRange)
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                state = NodeState.FAILURE;
            }
            return state;
        }
        else
        {
            return state = NodeState.FAILURE;
        }
    }
}
