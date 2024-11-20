using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashDistance = 5;
    public float dashDuration = 0.25f;
    bool dashIsActive = false;
    public void InitializeDash(Vector3 playerLocation, Vector3 targetLocation)
    {       
         Vector3 vectorDist = targetLocation - playerLocation;
         vectorDist.Normalize();
         targetLocation = playerLocation + (dashDistance * vectorDist);
        
         if (!dashIsActive) StartCoroutine(DashActive(playerLocation, targetLocation));
    }

    IEnumerator DashActive(Vector3 playerLocation, Vector3 targetLocation)
    {
        var t = 0f;
        dashIsActive = true;
        while (t < 1)
        {
            t += Time.deltaTime / dashDuration;
            if (t > 1) { t = 1; dashIsActive = false; }

            transform.position = Vector3.Lerp(playerLocation, targetLocation, t);

            yield return null;
        }
    }
}
