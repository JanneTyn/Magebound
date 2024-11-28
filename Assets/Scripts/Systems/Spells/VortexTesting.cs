using System.Collections;

using UnityEngine;

public class VortexTesting : MonoBehaviour
{
    public GameObject fireVortexPrefab; // The vortex effect prefab
    public GameObject targetingCirclePrefab; // Visual indicator for targeting
    public float range = 10f;
    public LayerMask groundLayer;
    public float pullRadius = 5;
    public float pullForce = 10f;

    private GameObject targetingCircle; // Instance of the targeting circle
    private bool isTargeting = false; // Whether the player is in targeting mode
    private Vector3 lastValidPosition; // Stores the last valid position for the targeting circle

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateTargetingMode();
        }

        if (isTargeting && Input.GetMouseButtonDown(0))
        {
            ConfirmTarget();
        }

        // Update the targeting circle position if in targeting mode
        if (isTargeting)
        {
            UpdateTargetingCircle();
        }*/
    }

    void ActivateTargetingMode()
    {
        if (targetingCircle == null)
        {
            targetingCircle = Instantiate(targetingCirclePrefab);
        }

        targetingCircle.SetActive(true);
        isTargeting = true;
        lastValidPosition = transform.position;
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

    void ConfirmTarget()
    {
        if (targetingCircle != null)
        {
            // Instantiate the ability prefab at the targeting circle's position
            GameObject vortexInstance = Instantiate(fireVortexPrefab, targetingCircle.transform.position, Quaternion.identity);

            StartCoroutine(VortexEffect(vortexInstance));
            
            Destroy(vortexInstance, 5f);

            Destroy(targetingCircle);
            isTargeting = false;
        }
    }

    IEnumerator VortexEffect(GameObject vortexInstance)
    {
        float duration = 5f;
        float timer = 0f;

        while (timer < duration) {

            if (vortexInstance == null)
            {
                yield break;
            }

            timer += Time.deltaTime;

            //Find all objects within the pull radius
            Collider[] hitColliders = Physics.OverlapSphere(vortexInstance.transform.position, pullRadius);

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 direction = (vortexInstance.transform.position - hit.transform.position).normalized;
                        rb.AddForce(direction * pullForce * Time.deltaTime, ForceMode.VelocityChange);
                    }
                }
            }

            yield return null;
        }
    }
}
