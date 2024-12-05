using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashDistance = 5;
    public float dashDuration = 0.25f;
    public bool dashIsActive = false;
    public GameObject DashFirePrefab;
    public GameObject DashIcePrefab;
    public GameObject DashThunderPrefab;
    public GameObject DashToUse;
    RaycastHit m_Hit;
    bool m_HitDetect;
    Vector3 collisionCheck;
    public void InitializeDash(Vector3 playerLocation, Vector3 targetLocation, int element)
    {       
         Vector3 vectorDist = targetLocation - playerLocation;
         vectorDist.Normalize();
         targetLocation = playerLocation + (dashDistance * vectorDist);
        
        if (!dashIsActive)
        {
            switch (element)
            {
                case 1:
                    GameObject dashEffect = Instantiate(DashFirePrefab, playerLocation, Quaternion.identity);
                    dashEffect.transform.position = new Vector3(playerLocation.x, 1, playerLocation.z);
                    StartCoroutine(FireDashActive(playerLocation, targetLocation, dashEffect));
                    break;
                case 2:
                    GameObject dashEffectIce = Instantiate(DashIcePrefab, playerLocation, Quaternion.identity);
                    dashEffectIce.transform.position = new Vector3(playerLocation.x, 0.1f, playerLocation.z);
                    StartCoroutine(IceDashActive(playerLocation, targetLocation, dashEffectIce));
                    break;
                case 3:                   
                    StartCoroutine(ThunderDashActive(playerLocation, targetLocation));
                    break;

                default:
                    Debug.Log("Invalid element, using fire instead");
                    DashToUse = DashFirePrefab; break;
            }

            
        }
    }

    IEnumerator FireDashActive(Vector3 playerLocation, Vector3 targetLocation, GameObject dashEffect)
    {
        var t = 0f;
        dashIsActive = true;
        transform.LookAt(targetLocation);
        dashEffect.transform.LookAt(targetLocation);
        dashEffect.transform.Rotate(0, 180, 0);
        while (t < 1)
        {
            t += Time.deltaTime / dashDuration;
            if (t > 1) { t = 1; dashIsActive = false; }

            collisionCheck = Vector3.Lerp(playerLocation, targetLocation, t);
            if (DashCollisionRayCast(collisionCheck)) { dashIsActive = false; break; }

            transform.position = collisionCheck;
            dashEffect.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

            yield return null;
        }
        dashEffect.GetComponent<BoxCollider>().enabled = true; //to prevent dmg happening in front of player during dash
        dashEffect.GetComponent<SpellDash>().dashFinished = true;
    }

    IEnumerator IceDashActive(Vector3 playerLocation, Vector3 targetLocation, GameObject dashEffect)
    {
        var t = 0f;
        dashIsActive = true;
        transform.LookAt(targetLocation);
        while (t < 1)
        {
            t += Time.deltaTime / dashDuration;
            if (t > 1) { t = 1; dashIsActive = false; }

            collisionCheck = Vector3.Lerp(playerLocation, targetLocation, t);
            if (DashCollisionRayCast(collisionCheck)) { dashIsActive = false; break; }

            transform.position = collisionCheck;

            yield return null;
        }
    }

    IEnumerator ThunderDashActive(Vector3 playerLocation, Vector3 targetLocation)
    {
        var t = 0f;
        dashIsActive = true;
        transform.LookAt(targetLocation);
        while (t < 1)
        {
            t += Time.deltaTime / dashDuration;
            if (t > 1) { t = 1; dashIsActive = false; }

            collisionCheck = Vector3.Lerp(playerLocation, targetLocation, t);
            if (DashCollisionRayCast(collisionCheck)) { dashIsActive = false; break; }

            transform.position = collisionCheck;

            yield return null;
        }
        GameObject dashEffectThunder = Instantiate(DashThunderPrefab, transform.position, Quaternion.identity);
        dashEffectThunder.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }

    bool DashCollisionRayCast(Vector3 pos)
    {
        float colliderRadius = 0.5f;
        return Physics.CheckSphere(pos, colliderRadius, LayerMask.GetMask("Wall"), QueryTriggerInteraction.Ignore);
    }

}
