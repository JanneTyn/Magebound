using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
public class SpellVortex : MonoBehaviour
{
    public Transform player;
    public GameObject fireVortexVFX;
    public GameObject iceVortexVFX;
    public GameObject lightningVortexVFX;
    public GameObject targetingCirclePrefab; // Visual indicator for targeting
    public float range = 10f;
    public LayerMask groundLayer;
    public float pullRadius = 5f;
    public float pullForce = 10f;

    private GameObject targetingCircle; // Instance of the targeting circle
    private Vector3 lastValidPosition; // Stores the last valid position for the targeting circle
    private bool isTargeting = false; // Whether the player is in targeting mode or not
    int playerElement;

    public void StartTargeting()
    {
        if (targetingCircle == null)
        {
            targetingCircle = Instantiate(targetingCirclePrefab);
        }

        targetingCircle.SetActive(true);
        isTargeting = true;
        lastValidPosition = transform.position;
    }

    public void PrepareAttackAnim()
    {
        StartCoroutine(GetComponent<PlayerAnimations>().InitializeAttackAnimation(targetingCircle.transform.position, 5));
    }

    public void ConfirmTarget()
    {       
        playerElement = player.GetComponent<CharacterStats_PlayerStats>().GetCurrentElement();
        if (isTargeting && targetingCircle != null)
        {
            GameObject vortexInstance = null;
            // Instantiate the ability prefab at the targeting circle's position
            switch (playerElement)
            {
                case 1:
                    vortexInstance = Instantiate(fireVortexVFX, targetingCircle.transform.position, Quaternion.identity);
                    break;
                case 2:
                    vortexInstance = Instantiate(iceVortexVFX, targetingCircle.transform.position, Quaternion.identity);
                    break;
                case 3:
                    vortexInstance = Instantiate(lightningVortexVFX, targetingCircle.transform.position, Quaternion.identity);
                    break;
                default:
                    Debug.LogWarning("SpellVortex.cs: No vortex instantiated.");
                    break;

            }
            //GameObject vortexInstance = Instantiate(fireVortexPrefab, targetingCircle.transform.position, Quaternion.identity);
            StartCoroutine(VortexEffect(vortexInstance));
            Destroy(vortexInstance, 10f);
            Destroy(targetingCircle);
            isTargeting = false;
        }
    }

    private void Update()
    {
        if (isTargeting)
        {
            UpdateTargetingCircle();
        }
    }

    void UpdateTargetingCircle()
    {
        // Raycast from the mouse cursor to the ground
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Ensure the raycast hits the ground
            Vector3 targetPosition = hit.point;

            // Check distance from the player
            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance <= range)
            {
                // Update position to target location within range
                targetingCircle.transform.position = targetPosition;
                lastValidPosition = targetPosition; // Update last valid position
            }
            else
            {
                // Clamp position to max range
                Vector3 direction = (targetPosition - transform.position).normalized;
                Vector3 clampedPosition = transform.position + direction * range;
                targetingCircle.transform.position = clampedPosition;
                lastValidPosition = clampedPosition; // Update last valid position
            }
        }
        else
        {
            // If the raycast doesn't hit the ground, keep the circle at the last valid position
            targetingCircle.transform.position = lastValidPosition;
        }
    }

    public void CancelTargeting()
    {
        if (targetingCircle != null)
        {
            Destroy(targetingCircle);
        }

        isTargeting = false;
    }

    IEnumerator VortexEffect(GameObject vortexInstance)
    {
        float duration = 10f; // Duration of the vortex effect
        float timer = 0f;

        List<NavMeshAgent> disabledAgents = new List<NavMeshAgent>(); //Keep track of enemy NavMeshAgent disabling & re-enabling
        List<Rigidbody> affectedRigidbodies = new List<Rigidbody>(); //Keep track of enemy Rigidbodies disabling & re-enabling rigidbodies freeze positions
        HashSet<GameObject> processedEnemies = new HashSet<GameObject>(); //Enemies that have vortex dot already
        List<EnemyAttack> affectedEnemies = new List<EnemyAttack>(); //Keep track of enemies' attack states

        while (timer < duration)
        {
            if (vortexInstance == null)
            {
                RestoreAgentsAndRigidbodies(disabledAgents, affectedRigidbodies); //Enemies NavMeshAgents and Rigidbodies
                RestoreEnemyAttackStates(affectedEnemies);
                yield break;
            }

            timer += Time.deltaTime;

            // Find all objects within the pull radius
            Collider[] hitColliders = Physics.OverlapSphere(vortexInstance.transform.position, pullRadius);
            HashSet<GameObject> currentAffectedEnemies = new HashSet<GameObject>();

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Enemy"))
                {
                    GameObject enemy = hit.gameObject;
                    currentAffectedEnemies.Add(enemy);
                    var enemyStats = hit.GetComponent<CharacterStats_EnemyStats>();

                    if (enemyStats != null && enemyStats.IsDead())
                    {
                        // Skip dead enemies
                        continue;
                    }

                    Rigidbody rb = hit.GetComponentInChildren<Rigidbody>();
                    NavMeshAgent agent = hit.GetComponentInChildren<NavMeshAgent>();
                    EnemyAttack enemyAttack = hit.GetComponent<EnemyAttack>();
                    
                    if (!processedEnemies.Contains(enemy))
                    {
                       processedEnemies.Add(enemy);

                       //Disable enemy attack behavior while in vortex
                       if (enemyAttack != null)
                       {
                            enemyAttack.SetCanAttack(false);
                            if (!affectedEnemies.Contains(enemyAttack))
                            {
                                affectedEnemies.Add(enemyAttack);
                            }
                       }

                       DamageSystem damageSystem = enemy.GetComponent<DamageSystem>();
                       if (damageSystem != null) {
                            int statusID = 0;
                            switch (playerElement)
                            {
                                case 1: // Fire vortex
                                    statusID = 1; // Burn
                                    break;
                                case 2: // Ice vortex
                                    statusID = 3; // Chill
                                    break;
                                case 3: // Lightning vortex
                                    statusID = 4; // Stun
                                    break;
                                default:
                                    Debug.LogWarning("Invalid vortex element.");
                                    break;
                            }

                            if (statusID != 0) {
                                damageSystem.CalculateDamage(0, true, statusID, 5f, 10f, playerElement);
                            }
                        }
                    }

                    if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                    {
                        // Disable the NavMeshAgent temporarily for the pull effect
                        //agent.enabled = false;

                        agent.isStopped = true;

                        // Add the agent to the list if not already added
                        if (!disabledAgents.Contains(agent))
                        {
                            disabledAgents.Add(agent);
                        }

                        Vector3 directionToCenter = (vortexInstance.transform.position - agent.transform.position).normalized;
                        Vector3 tangentialDirection = Vector3.Cross(directionToCenter, Vector3.up).normalized;
                        float spinForce = Mathf.Lerp(pullForce * 0.2f, 0, Vector3.Distance(vortexInstance.transform.position, agent.transform.position) / pullRadius);
                        Vector3 vortexForce = tangentialDirection * spinForce + directionToCenter * (pullForce * 0.1f);
                        agent.velocity = vortexForce;
                        if (agent.velocity.magnitude > 3f)
                        {
                            agent.velocity = agent.velocity.normalized * 3f;
                        }
                    }

                    // Apply the pull force
                    if (rb != null)
                    {
                        //Remove rigidbody constraints so the vortex pull effext is possible
                        if (!affectedRigidbodies.Contains(rb))
                        {
                            affectedRigidbodies.Add(rb);
                            rb.constraints = RigidbodyConstraints.FreezePositionY;
                        }

                        Vector3 directionToCenter = (vortexInstance.transform.position - rb.position).normalized;
                        Vector3 tangentialDirection = Vector3.Cross(directionToCenter, Vector3.up).normalized;
                        float spinForce = Mathf.Lerp(pullForce * 0.5f, 0, Vector3.Distance(vortexInstance.transform.position, rb.position) / pullRadius);
                        rb.AddForce(tangentialDirection * spinForce, ForceMode.Acceleration);
                        rb.AddForce(directionToCenter * (pullForce * 0.1f), ForceMode.Acceleration);
                        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 3f);
                    }
                }
                else if (hit.CompareTag("SpellEffect"))
                {
                    if (hit.TryGetComponent<SpellEffect_CrystalShard_Fire>(out SpellEffect_CrystalShard_Fire shard))
                    {
                        Rigidbody rb = hit.GetComponentInChildren<Rigidbody>();

                        if (playerElement == 3)
                        {
                            shard.GetComponent<VisualEffect>().SetBool("Supercharged", true);
                            shard.GetComponent<VisualEffect>().SetFloat("ExplosionSize", 20);
                            shard.explosionSize = 10;
                        }

                        if (rb != null)
                        {
                            //Remove rigidbody constraints so the vortex pull effext is possible
                            if (!affectedRigidbodies.Contains(rb))
                            {
                                affectedRigidbodies.Add(rb);
                                rb.constraints = RigidbodyConstraints.FreezePositionY;
                            }

                            Vector3 directionToCenter = (vortexInstance.transform.position - rb.position).normalized;
                            Vector3 tangentialDirection = Vector3.Cross(directionToCenter, Vector3.up).normalized;
                            float spinForce = Mathf.Lerp(pullForce * 0.5f, 0, Vector3.Distance(vortexInstance.transform.position, rb.position) / pullRadius);
                            rb.AddForce(tangentialDirection * spinForce, ForceMode.Acceleration);
                            rb.AddForce(directionToCenter * (pullForce * 0.1f), ForceMode.Acceleration);
                            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 3f);
                        }
                    }
                    else if (hit.TryGetComponent<SpellEffect_CrystalShard_Ice>(out SpellEffect_CrystalShard_Ice iceshard) || hit.TryGetComponent<SpellEffect_CrystalShard_Thunder>(out SpellEffect_CrystalShard_Thunder thundershard))
                    {
                        Rigidbody rb = hit.GetComponentInChildren<Rigidbody>();

                        if (rb != null)
                        {
                            //Remove rigidbody constraints so the vortex pull effext is possible
                            if (!affectedRigidbodies.Contains(rb))
                            {
                                affectedRigidbodies.Add(rb);
                                rb.constraints = RigidbodyConstraints.FreezePositionY;
                            }

                            Vector3 directionToCenter = (vortexInstance.transform.position - rb.position).normalized;
                            Vector3 tangentialDirection = Vector3.Cross(directionToCenter, Vector3.up).normalized;
                            float spinForce = Mathf.Lerp(pullForce * 0.5f, 0, Vector3.Distance(vortexInstance.transform.position, rb.position) / pullRadius);
                            rb.AddForce(tangentialDirection * spinForce, ForceMode.Acceleration);
                            rb.AddForce(directionToCenter * (pullForce * 0.1f), ForceMode.Acceleration);
                            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 3f);
                        }
                    }
                    else if (hit.TryGetComponent<SpellEffect_Explosion_Ice>(out SpellEffect_Explosion_Ice iceExplosion))
                    {
                        if (playerElement == 1)
                        {
                            Destroy(iceExplosion.gameObject);
                            Vector3 spawnPosition = new Vector3(vortexInstance.transform.position.x, vortexInstance.transform.position.y + 1, vortexInstance.transform.position.z);
                            Instantiate(Resources.Load<GameObject>("Synergies/SteamCloud"), spawnPosition, Quaternion.identity);
                            Destroy(vortexInstance);
                        }
                    }
                    else
                    {
                        Debug.Log("No valid synergy found");
                    }
                }
            }
            List<NavMeshAgent> toRestore = new List<NavMeshAgent>();
            foreach (NavMeshAgent agent in disabledAgents)
            {
                if (agent != null && (!currentAffectedEnemies.Contains(agent.gameObject) || Vector3.Distance(agent.transform.position, vortexInstance.transform.position) > pullRadius))
                {
                    // Agent is outside the vortex or no longer in range
                    agent.isStopped = false;
                    agent.velocity = Vector3.zero;
                    agent.SetDestination(player.position);
                    toRestore.Add(agent);
                }
            }

            foreach (NavMeshAgent restoredAgent in toRestore)
            {
                disabledAgents.Remove(restoredAgent);
            }

            yield return null;
        }

        processedEnemies.Clear();

        // Re-enable NavMeshAgents and restore Rigidbody constraints after the vortex effect ends
        RestoreAgentsAndRigidbodies(disabledAgents, affectedRigidbodies);
        //Restore attack states
        RestoreEnemyAttackStates(affectedEnemies);
    }

    private void RestoreAgentsAndRigidbodies(List<NavMeshAgent> agents, List<Rigidbody> rbs)
    {
        foreach (NavMeshAgent agent in agents)
        {
            /*if (agent != null)
            {
                agent.enabled = true;
                agent.SetDestination(player.position);
            }*/

            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                try
                {
                    agent.isStopped = false; // Resume normal pathfinding
                    agent.velocity = Vector3.zero; // Clear any residual velocity
                    agent.SetDestination(player.position); // Recalculate path if needed

                    //Set Animator "IsMoving" parameter
                    Animator animator = agent.GetComponentInChildren<Animator>();
                    if (animator != null)
                    {
                        bool isMoving = agent.velocity.magnitude > 0.1f;
                        animator.SetBool("IsMoving", isMoving);
                    }
                }
                catch (System.Exception e) {
                    Debug.LogWarning($"Error restoring NavMeshAgent: {agent.gameObject.name}. Exception: {e.Message}");
                }
            }

            foreach (Rigidbody rb in rbs)
            {
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.FreezePosition;
                }
            }
        }
    }

    private void RestoreEnemyAttackStates(List<EnemyAttack> enemies)
    {
        foreach(var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetCanAttack(true);
            }
        }
    }

    public bool IsTargetingActive()
    {
        return isTargeting;
    }
}
