using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SpellDash : SpellEffect
{
    public bool dashFinished = false;
    public bool trailLifeWait = false;
    private string isDashingProperty = "IsDashing";
    private void Update()
    {
        if (dashFinished && !trailLifeWait)
        {
            GetComponent<VisualEffect>().SetBool(isDashingProperty, false);
            trailLifeWait = true;
        }
        else if (dashFinished && trailLifeWait)
        {
            StartCoroutine(TrailDuration());
            dashFinished = false;
        }
    }

    IEnumerator TrailDuration()
    {
        float trailLife = GetComponent<VisualEffect>().GetFloat("TrailLife") - 1;
        float t = 0;

        while (t < trailLife)
        {
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    public override void CheckOverlap(int o)
    {
        throw new System.NotImplementedException();
    }

    public override void Activate(int o)
    {
        throw new System.NotImplementedException();
    }
}
