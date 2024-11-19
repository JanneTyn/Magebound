using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class SpellBaseEffect : MonoBehaviour
{

    public GameObject BasicAttackFirePrefab;
    public VisualEffectAsset PreLoadBasicAttackFirePrefab;
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
        //var VFX = Resources.Load<VisualEffectAsset>("VFX/BasicProjectile/BasicProjectileFire");
        //attack.GetComponent<VisualEffect>().visualEffectAsset = VFX;
    }
}
