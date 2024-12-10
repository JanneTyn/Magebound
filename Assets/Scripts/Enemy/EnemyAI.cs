using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private EnemyAttack enemyAttack;
    public GameObject playerObject;

    private BTNode currentBehaviorTree;

    public float detectionRange = 100f;
    public float rotationSpeed = 5;
    public float attackRange = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();

        GameManager.Instance.AddEnemy(gameObject);

        if (GameObject.FindWithTag("Player").GetComponent<StatusManager>().isObscured)
        {
            playerObject = null;
        }else
        {
            playerObject = GameObject.FindWithTag("Player");
        }

            
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
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            if (animator.parameters != null && AnimatorHasParameter(animator, "IsDead"))
            {
                if (animator.GetBool("IsDead"))
                {
                    //Stop behavior tree evaluation if enemy is dead
                    return;
                }
            }

            if (!AnimatorHasParameter(animator, "IsDead"))
            {
                Debug.LogWarning("Animator is missing parameter 'IsDead' on object: " + gameObject.name);
            }
        }

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

    //For checking if an Animator has a specific parameter
    private bool AnimatorHasParameter(Animator animator, string paramName)
    {
        foreach (var param in animator.parameters) {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
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