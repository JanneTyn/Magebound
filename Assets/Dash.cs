using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashDistance = 5;
    public float dashDuration = 0.25f;
    public bool dashIsActive = false;
    public GameObject DashFirePrefab;
    public void InitializeDash(Vector3 playerLocation, Vector3 targetLocation)
    {       
         Vector3 vectorDist = targetLocation - playerLocation;
         vectorDist.Normalize();
         targetLocation = playerLocation + (dashDistance * vectorDist);

        GameObject dashEffect = Instantiate(DashFirePrefab, playerLocation, Quaternion.identity);
        if (!dashIsActive) StartCoroutine(DashActive(playerLocation, targetLocation, dashEffect));
    }

    IEnumerator DashActive(Vector3 playerLocation, Vector3 targetLocation, GameObject dashEffect)
    {
        var t = 0f;
        dashIsActive = true;
        transform.LookAt(targetLocation);
        while (t < 1)
        {
            t += Time.deltaTime / dashDuration;
            if (t > 1) { t = 1; dashIsActive = false; }

            transform.position = Vector3.Lerp(playerLocation, targetLocation, t);
            dashEffect.transform.position = transform.position;

            yield return null;
        }
    }
}
