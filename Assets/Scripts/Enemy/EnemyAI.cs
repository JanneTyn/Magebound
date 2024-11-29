using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private EnemyAttack enemyAttack;

    private BTNode currentBehaviorTree;

    public float detectionRange = 100f;
    public float rotationSpeed = 5;
    public float attackRange = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null )
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("EnemyAI.cs: Player not found. Make sure the player GameObject has the correct tag.");
            enabled = false; // Disable the script if player is not found
            return;
        }

        BuildBehaviorTree();
    }

    void Update()
    {
        if (currentBehaviorTree != null)
        {
            BTNode.NodeState state = currentBehaviorTree.Evaluate();

            // Optionally, handle the state (like logging or additional checks)
            if (state == BTNode.NodeState.RUNNING)
            {
                //Debug.Log("AI is running.");
            }
            else if (state == BTNode.NodeState.SUCCESS)
            {
                //Debug.Log("AI successfully completed action");
            }
            else if (state == BTNode.NodeState.FAILURE)
            {
                //Debug.Log("AI failed to perform action.");
            }
        }
    }

    void BuildBehaviorTree()
    {
        //Condition nodes
        var isPlayerInSight = new IsPlayerInSight(transform, player, detectionRange, rotationSpeed);
        var isPlayerInAttackRange = new IsPlayerInAttackRange(transform, player, attackRange);

        //Action nodes
        var moveToPlayer = new MoveToPlayer(agent, player);
        var attackPlayer = new AttackPlayer(enemyAttack);

        //Sequences for chasing & attacking
        var chaseSequence = new Sequence(new List<BTNode> { isPlayerInSight, moveToPlayer });
        var attackSequence = new Sequence(new List<BTNode> { isPlayerInAttackRange,  attackPlayer});

        //Selector
        currentBehaviorTree = new Selector(new List<BTNode> { chaseSequence, attackSequence });
    }
}