using UnityEngine;

public class IsPlayerInSight : BTNode
{
    private Transform enemy;
    private Transform player;
    private float detectionRange;
    private float rotationSpeed;

    public IsPlayerInSight(Transform enemy, Transform player, float detectionRange, float rotationSpeed)
    {
        this.enemy = enemy;
        this.player = player;
        this.detectionRange = detectionRange;
        this.rotationSpeed = rotationSpeed;
    }

    public override NodeState Evaluate()
    {
        if(player != null)
        {
            float distance = Vector3.Distance(enemy.position, player.position);

            if (distance <= detectionRange)
            {
                Vector3 direction = (player.position - enemy.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                enemy.rotation = Quaternion.Slerp(enemy.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
