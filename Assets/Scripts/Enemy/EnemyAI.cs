using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private float detectionRange = 100f;
    private float moveSpeed = 5f;
    private float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find player GameObject by its tag
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("EnemyAI.cs: Player not found. Make sure the player GameObject has the correct tag.");
            enabled = false; //Disable the script if player is not found
            return;
        }

        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("EnemyAI.cs: NavMeshAgent component is missing from Enemy GameObject", this);
            enabled = false; //Disable the script if NavMeshAgent component is missing from Enemy GameObject
            return;
        }

        //Agents movement and rotation speeds
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent == null)
        {
            Debug.Log("EnemyAI.cs: agent is null.");
            return;
        }
        if (player == null)
        {
            Debug.Log("EnemyAI.cs: player is null.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Debug.Log($"Enemy distance to player: {distanceToPlayer}");

        if (distanceToPlayer < detectionRange)
        {
            //Set players position as the destination
            agent.SetDestination(player.position);
        }

        /*else
        {
            //Stop the agent if out of detection range
            agent.ResetPath();
        }*/

        RotateTowardsPlayer();
    }

    void RotateTowardsPlayer()
    {
        //Calculate the direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        //Keep the rotation on the Y - axis (ignore vertical rotation)
        directionToPlayer.y = 0f;

        //Lerp or Slerp can be used for smooth rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        //For instant rotation, this can be used:
        //transform.rotation = targetRotation;

        //For smooth rotation, this can be used:
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
