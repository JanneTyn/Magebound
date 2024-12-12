using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageFlash : MonoBehaviour
{
    //Renderer rendererComponent;
    //Color origColor;
    private Renderer[] renderers;
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    float flashTime = 0.5f;

    void Start()
    {
        //rendererComponent = GetComponentInChildren<Renderer>();
        renderers = GetComponentsInChildren<Renderer>();

        /*if (rendererComponent == null)
        {
            Debug.LogError("DamageFlash.cs: No Renderer found on " + gameObject.name);
            return;
        }*/

        if (renderers.Length == 0)
        {
            Debug.LogError("DamageFlash.cs: No renderers found in " + gameObject.name + " or its children.");
            return;
        }

        /*rendererComponent.material = new Material(rendererComponent.material);
        origColor = rendererComponent.material.color;*/

        foreach (var rend in renderers)
        {
            rend.material = new Material(rend.material);
            originalColors[rend] = rend.material.color;
        }
    }

    public IEnumerator EFlash()
    {
        //Debug.Log("EFlash triggered on: " + gameObject.name);

        /*if (rendererComponent == null) {
            Debug.LogError("EFlash failed: No renderer found on " + gameObject.name);
            yield break;
        }*/

        if (renderers.Length == 0)
        {
            Debug.LogError("Eflash failed: No renderers found on " + gameObject.name);
        }

        float elapsedTime = 0f;
        float halfFlashTime = flashTime / 2f;

        //Fade in to red
        while (elapsedTime < halfFlashTime)
        {
            /*rendererComponent.material.color = Color.Lerp(origColor, Color.red, elapsedTime / halfFlashTime);
            elapsedTime += Time.deltaTime;
            yield return null;*/

            foreach (var rend in renderers)
            {
                rend.material.color = Color.Lerp(originalColors[rend], Color.red, elapsedTime / halfFlashTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //rendererComponent.material.color = Color.red;
        foreach (var rend in renderers)
        {
            rend.material.color = Color.red;
        }

        elapsedTime = 0f;

        //Fade out to original color
        while (elapsedTime < halfFlashTime)
        {
            /*rendererComponent.material.color = Color.Lerp(Color.red, origColor, elapsedTime / halfFlashTime);
            elapsedTime += Time.deltaTime;
            yield return null;*/

            foreach (var rend in renderers)
            {
                rend.material.color = Color.Lerp(Color.red, originalColors[rend], elapsedTime / halfFlashTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //rendererComponent.material.color = origColor;

        foreach (var rend in renderers)
        {
            rend.material.color = originalColors[rend];
        }
    }
}
