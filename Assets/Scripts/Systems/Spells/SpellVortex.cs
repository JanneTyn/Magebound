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

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Enemy"))
                {
                    //int enemyElement = hit.GetComponent<CharacterStats_EnemyStats>().GetCurrentElement();
                    
                    Rigidbody rb = hit.GetComponentInChildren<Rigidbody>();
                    NavMeshAgent agent = hit.GetComponentInChildren<NavMeshAgent>();
                    EnemyAttack enemyAttack = hit.GetComponent<EnemyAttack>();
                    GameObject enemy = hit.gameObject;

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

                    if (agent != null && agent.enabled)
                    {
                        // Disable the NavMeshAgent temporarily for the pull effect
                        agent.enabled = false;

                        //Add the enemy to the list if not already added
                        if (!disabledAgents.Contains(agent))
                        {
                            disabledAgents.Add(agent);
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
                        Destroy(iceExplosion.gameObject);
                        Vector3 spawnPosition = new Vector3(vortexInstance.transform.position.x, vortexInstance.transform.position.y + 1, vortexInstance.transform.position.z);
                        Instantiate(Resources.Load<GameObject>("Synergies/SteamCloud"), spawnPosition, Quaternion.identity);
                        Destroy(vortexInstance);
                    }
                    else
                    {
                        Debug.Log("No valid synergy found");
                    }
                }
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
            if (agent != null)
            {
                agent.enabled = true;
                agent.SetDestination(player.position);
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
