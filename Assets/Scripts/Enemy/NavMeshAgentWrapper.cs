using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentWrapper : MonoBehaviour, IStatusVariables
{
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public float speed
    {
        get => agent.speed;
        set => agent.speed = value;
    }
}
