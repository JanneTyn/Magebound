using System.Collections;

using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float attackRange = 2f;
    private float attackCooldown = 3f;
    private float lastAttackTime = 0f;
    private float attackDamage = 50f;

    private Transform player;
    public LayerMask playerLayerMask;
    private Animator animator;

    public Vector3 attackSize = new Vector3(1f, 1f, 2f);
    public Vector3 attackCenterOffset = new Vector3(0f, 1f, 0f);

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.LogWarning("EnemyAttack.cs: Player not found!");
            return;
        }

        if (animator == null)
        {
            Debug.LogWarning("EnemyAttack.cs: Animator == null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Debug.Log("EnemyAttack.cs: Calling Attack() method");
                Attack();
            }
        }
    }

    public void Attack()
    {
        TriggerAttackAnimation();
        lastAttackTime = Time.time;
    }

    private void TriggerAttackAnimation()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(CheckForPlayerInAttackArea());
    }

    private IEnumerator CheckForPlayerInAttackArea()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 attackCenter = transform.position + attackCenterOffset;
        Collider[] hitColliders = Physics.OverlapBox(attackCenter, attackSize / 2, Quaternion.identity, playerLayerMask);
        //Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, playerLayerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                yield return new WaitForSeconds(0.25f);
                hitCollider.GetComponent<DamageSystem>().CalculateDamage(attackDamage, false, 0, 2);
                Debug.Log($"Player hit and damaged! ({attackDamage} HP)");

                //DamageSystem playerDamageSystem = hitCollider.GetComponent<DamageSystem>();
                /*if (playerDamageSystem != null)
                {
                    playerDamageSystem.CalculateDamage(attackDamage, false, 0, 2);
                    Debug.Log($"Player hit and damaged! ({attackDamage} HP)");
                }*/
            }
            else
            {
                Debug.LogWarning("EnemyAttack.cs: Player does not have a DamageSystem component.");
            }
        }
    }
}