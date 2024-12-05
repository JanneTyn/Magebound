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
                //StartCoroutine(BurningGround(targetLocation, explosion));
                break;
            case 2:
                GameObject iceExplosion = Instantiate(ExplosionIcePrefab, playerLocation, Quaternion.identity);
                iceExplosion.GetComponent<SpellEffect_Explosion_Ice>().SetProjectileDirection(targetLocation, playerLocation);
                break;
            case 3:
                GameObject thunderExplosion = Instantiate(ExplosionThunderPrefab, playerLocation, Quaternion.identity);
                thunderExplosion.GetComponent<SpellEffect_Explosion_Thunder>().SetProjectileDirection(targetLocation, playerLocation);
                break;
        }
    }  

    IEnumerator BurningGround(Vector3 targetLocation, GameObject explosion)
    {
        float t = 0;

        while (explosion.GetComponent<SpellEffect_Explosion_Fire>().explosionFin == false)
        {
            yield return null;
        }
        GameObject dashEffect = Instantiate(DashFirePrefab, explosion.transform.position, Quaternion.identity);
        dashEffect.transform.position = new Vector3(explosion.transform.position.x, 0, explosion.transform.position.z);
        StartCoroutine(dashEffect.GetComponent<SpellDash>().TrailDuration());
        dashEffect.GetComponent<BoxCollider>().enabled = true;
        dashEffect.GetComponent<VisualEffect>().SetFloat("TrailWidth", 4.5f);
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            yield return null;
        }
        explosion.GetComponent<SpellEffect_Explosion_Fire>().groundSet = true;

    }
}
