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
        agent.SetDestination(player.position);

        if (Vector3.Distance(agent.transform.position, player.position) <= agent.stoppingDistance)
        {
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.RUNNING;
        }
        return state;
    }
}
