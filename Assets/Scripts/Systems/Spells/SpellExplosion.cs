using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SpellExplosion : MonoBehaviour
{
    public GameObject ExplosionFirePrefab;
    public GameObject ExplosionIcePrefab;
    public GameObject ExplosionThunderPrefab;
    public void InitializeExplosion(Vector3 playerLocation, Vector3 targetLocation, int element)
    {
        switch (element)
        {
            case 1:
                GameObject explosion = Instantiate(ExplosionFirePrefab, targetLocation, Quaternion.identity);
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }  
}
