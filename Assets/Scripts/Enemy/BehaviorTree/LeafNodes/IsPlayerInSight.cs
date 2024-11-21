using UnityEngine;

public class IsPlayerInSight : BTNode
{
    private Transform enemy;
    private Transform player;
    private float detectionRange;

    public IsPlayerInSight(Transform enemy, Transform player, float detectionRange)
    {
        this.enemy = enemy;
        this.player = player;
        this.detectionRange = detectionRange;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(enemy.position, player.position);
        if (distance <= detectionRange)
        {
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
