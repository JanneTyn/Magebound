using System.Collections;
using UnityEngine;
public abstract class EnemyAttack : MonoBehaviour
{
    protected float attackRange = 2f;
    protected float  attackCooldown = 3f;
    protected float lastAttackTime = 0f;

    protected Transform player;
    public LayerMask playerLayerMask;
    protected Animator animator;

    public Vector3 attackSize = new Vector3(1f, 1f, 2f);
    public Vector3 attackCenterOffset = new Vector3(0f, 1f, 0f);

    public float damage = 50f;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.LogWarning("EnemyAttack.cs: Player not found!");
        }

        if (animator == null)
        {
            Debug.LogWarning("EnemyAttack.cs: Animator not found!");
        }
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
            }
        }

    }

    public virtual void Attack()
    {
        TriggerAttackAnimation();
        lastAttackTime = Time.time;
    }

    protected void TriggerAttackAnimation()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(CheckForPlayerInAttackArea());
    }

    protected virtual IEnumerator CheckForPlayerInAttackArea()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 attackCenter = transform.position + attackCenterOffset;
        Collider[] hitColliders = Physics.OverlapBox(attackCenter, attackSize / 2, Quaternion.identity, playerLayerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                yield return new WaitForSeconds(0.1f);
                HandlePlayerHit(hitCollider);
            }
        }
    }

    protected abstract void HandlePlayerHit(Collider player);

    //DEBUGGING: Draw the attack size in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackCenter = transform.position + attackCenterOffset;
        Gizmos.DrawWireCube(attackCenter, attackSize);
    }
}