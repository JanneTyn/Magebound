using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SpellEffect_Explosion_Fire : SpellEffect_Explosive
{
    float time = 0;
    float explosionDamageDelayTime = 0.3f;
    float explosionEffectTime = 1f;
    bool dmgActive = false;
    bool explosionFin = false;
    
    // Update is called once per frame
    void Update()
    {
        if (time > explosionDamageDelayTime && !dmgActive)
        {
            GetComponent<BoxCollider>().enabled = true;
            dmgActive = true;
        }
        if (time > explosionEffectTime && !explosionFin)
        {
            GetComponent<MeshRenderer>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(TrailDuration());
            explosionFin = true;
        }
        time += Time.deltaTime;
    }

    IEnumerator TrailDuration()
    {
        float trailLife = GetComponentInChildren<VisualEffect>().GetFloat("TrailLife") - 1;
        float t = 0;

        while (t < trailLife)
        {
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
