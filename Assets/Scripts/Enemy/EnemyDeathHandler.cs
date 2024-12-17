using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeathHandler : MonoBehaviour
{
    public void HandleDeath()
    {
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        GetComponent<EnemyAttack>().enabled = false;

        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            if (animator.HasState(0, Animator.StringToHash("Dead")))
            {
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            }
        }

        Destroy(gameObject);
        if(transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
