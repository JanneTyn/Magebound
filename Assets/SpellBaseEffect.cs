using UnityEngine;

public class SpellBaseEffect : MonoBehaviour
{

    public GameObject BasicAttackFirePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeProjectile(Vector3 playerLocation, Vector3 targetLocation)
    {
        Vector3 linedir = (targetLocation - playerLocation).normalized;
        Debug.DrawLine(playerLocation, playerLocation + linedir * 50, Color.red, Mathf.Infinity);

        GameObject attack = Instantiate(BasicAttackFirePrefab, playerLocation, Quaternion.identity);
        attack.GetComponent<SpellProjectile>().SetProjectileDirection(targetLocation, playerLocation);
    }
}
