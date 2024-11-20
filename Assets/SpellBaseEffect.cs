using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class SpellBaseEffect : MonoBehaviour
{

    public GameObject BasicAttackFirePrefab;
    public GameObject BasicAttackIcePrefab;
    public GameObject BasicAttackThunderPrefab;
    public GameObject BasicAttackToUse;
    public VisualEffectAsset PreLoadBasicAttackFirePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeProjectile(Vector3 playerLocation, Vector3 targetLocation, int element)
    {
        Vector3 linedir = (targetLocation - playerLocation).normalized;
        Debug.DrawLine(playerLocation, playerLocation + linedir * 50, Color.red, Mathf.Infinity);

        switch (element)
        {
            case 1:
                BasicAttackToUse = BasicAttackFirePrefab;
                break;
            case 2:
                BasicAttackToUse = BasicAttackIcePrefab;
                break;
            case 3:
                BasicAttackToUse = BasicAttackThunderPrefab;
                break;

            default:
                Debug.Log("Invalid element, using fire instead");
                BasicAttackToUse = BasicAttackFirePrefab; break;
        }
        
        GameObject attack = Instantiate(BasicAttackToUse, playerLocation, Quaternion.identity);
        attack.GetComponent<SpellProjectile>().SetProjectileDirection(targetLocation, playerLocation);
        //var VFX = Resources.Load<VisualEffectAsset>("VFX/BasicProjectile/BasicProjectileFire");
        //attack.GetComponent<VisualEffect>().visualEffectAsset = VFX;
    }
}
