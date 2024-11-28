using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SpellExplosion : MonoBehaviour
{
    public GameObject ExplosionFirePrefab;
    public GameObject ExplosionIcePrefab;
    public GameObject ExplosionThunderPrefab;
    public GameObject DashFirePrefab; //used for burning ground, replacable
    public void InitializeExplosion(Vector3 playerLocation, Vector3 targetLocation, int element)
    {
        switch (element)
        {
            case 1:
                GameObject explosion = Instantiate(ExplosionFirePrefab, playerLocation, Quaternion.identity);
                explosion.GetComponent<SpellEffect_Explosion_Fire>().SetProjectileDirection(targetLocation, playerLocation);
                StartCoroutine(BurningGround(targetLocation, explosion));
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }  

    IEnumerator BurningGround(Vector3 targetLocation, GameObject explosion)
    {
        float t = 0;

        while (explosion.GetComponent<SpellEffect_Explosion_Fire>().explosionFin == false)
        {
            t += Time.deltaTime;
            yield return null;
        }
        GameObject dashEffect = Instantiate(DashFirePrefab, targetLocation, Quaternion.identity);
        dashEffect.transform.position = new Vector3(targetLocation.x, 0, targetLocation.z);
        StartCoroutine(dashEffect.GetComponent<SpellDash>().TrailDuration());
        dashEffect.GetComponent<BoxCollider>().enabled = true;
        explosion.GetComponent<SpellEffect_Explosion_Fire>().groundSet = true;

    }
}
