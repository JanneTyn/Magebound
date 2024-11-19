using System.Collections;

using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float attackRange = 3f;
    private float attackCooldown = 3f;
    private float attackSpeed = 1f;
    private float lastAttackTime = 0f;
    private int attackDamage = 0;
    private Transform player;
    public GameObject hand;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("EnemyAttack.cs: Player not found!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange )
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    void AttackPlayer()
    {
        //Enemy attack animation & sound
        StartCoroutine(ActivateAndDeactivateHand());
        Debug.Log($"Enemy attacked player. Damage: {attackDamage}");

        //Player loses health, esim:
        //player.GetComponent<PlayerStats>().TakeDamage(attackDamage);
    }

    IEnumerator ActivateAndDeactivateHand()
    {
        if (hand != null)
        {
            hand.SetActive(true);
            yield return new WaitForSeconds(1);
            hand.SetActive(false);
        }
        else
        {
            Debug.LogError("EnemyAttack.cs: Hand GameObject not found.");
        }

    }
}
