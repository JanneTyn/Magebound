using UnityEngine;

public class EnemyHandOverlapCheck : MonoBehaviour
{
    public LayerMask playerLayerMask;

    public bool IsPlayerInRange()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, playerLayerMask);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}
