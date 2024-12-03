using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : BTNode
{
    private NavMeshAgent agent;
    private Transform player;

    public MoveToPlayer(NavMeshAgent agent, Transform player)
    {
        this.agent = agent;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        // Check if the NavMeshAgent is enabled and on a valid NavMesh
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(agent.transform.position, player.position) <= agent.stoppingDistance)
            {
                state = NodeState.SUCCESS;
            }
            else
            {
                state = NodeState.RUNNING;
            }
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}
