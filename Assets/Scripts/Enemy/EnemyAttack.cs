using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float attackRange = 3f;
    private float attackCooldown = 3f;
    private float lastAttackTime = 0f;
    private float attackDamage = 50f;
    
    private Transform player;
    public GameObject hand;
    public LayerMask playerLayerMask; //Might not be needed if everything is in the same layer

    private EnemyHandOverlapCheck handOverlapCheck; //Used for checking if enemies hand hits player
    private bool hasDamagedPlayer = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (hand != null)
        {
            handOverlapCheck = hand.GetComponent<EnemyHandOverlapCheck>();
            if (handOverlapCheck == null)
            {
                Debug.LogError("EnemyAttack.cs: EnemyHandOverlapCheck component not found on hand.");
            }
        }
        else
        {
            Debug.LogError("EnemyAttack.cs: Hand == null.");
        }
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
        StartCoroutine(ActivateAndDeactivateAttack());
        Debug.Log($"Enemy attacked player. Damage: {attackDamage}");
    }

    IEnumerator ActivateAndDeactivateAttack()
    {   
        //TODO:
        //Add animation
        //Delete hand.SetActive() stuff (after animation is added)

        if (hand != null)
        {
            hand.SetActive(true);

            if (handOverlapCheck != null)
            {
                if (handOverlapCheck.IsPlayerInRange() && !hasDamagedPlayer)
                {
                    ApplyDamage();
                }
            }

            yield return new WaitForSeconds(1); //Duration of attack
            hand.SetActive(false);
            ResetDamageFlag();
        }
        else
        {
            Debug.LogError("EnemyAttack.cs: Hand GameObject not found.");
        }
    }

    void ApplyDamage()
    {
        //Check for enemy & player states (Ice, fire or electric) before applying damage
        //Player loses health, esim:
        //player.GetComponent<PlayerStats>().TakeDamage(attackDamage);
        hasDamagedPlayer = true;
    }

    void ResetDamageFlag()
    {
        hasDamagedPlayer = false;
        Debug.Log("EnemyAttack.cs: Damage flag resetted.");
    }
}