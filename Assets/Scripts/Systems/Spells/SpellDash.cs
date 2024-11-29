using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SpellDash : SpellEffect
{
    public bool dashFinished = false;
    public bool trailLifeWait = false;
    private bool iceActivated = false;
    private bool thunderActivated = false;
    private string isDashingProperty = "IsDashing";
    private float dmgTimer = 0;
    private void Update()
    {
        switch(GetElementID())
        {
            case 1:
                FireTrail();
                break;
            case 2:
                IceFreeze();
                break;
            case 3:
                ThunderStun();
                break;
        }
    }

    private void FireTrail()
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

    private void IceFreeze()
    {
        if (!iceActivated) 
        {
            StartCoroutine(IceEffectDuration());
            iceActivated = true;
        }
    }

    private void ThunderStun()
    {
        if (!thunderActivated)
        {
            StartCoroutine(ThunderEffectDuration());
            thunderActivated = true;
        }
    }

    IEnumerator IceEffectDuration()
    {
        float effectLife = GetComponent<VisualEffect>().GetFloat("FreezeTime");
        Debug.Log("effectlife: " + effectLife);
        float t = 0;
        GetComponent<BoxCollider>().enabled = true;

        while (t < effectLife)
        {
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator ThunderEffectDuration()
    {
        float effectLife = GetComponent<VisualEffect>().GetFloat("StunTime");
        Debug.Log("effectlife: " + effectLife);
        float t = 0;
        GetComponent<BoxCollider>().enabled = true;

        while (t < effectLife)
        {
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    public IEnumerator TrailDuration()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (GetElementID() == 2 || GetElementID() == 3)
            {
                other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetElementID());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (GetElementID() == 1)
        {
            dmgTimer += Time.deltaTime;
            if (dmgTimer >= 0.5f)
            {
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<DamageSystem>().CalculateDamage(GetDamage(), GetElementID());
                }
                dmgTimer = 0;
            }
        }
    }

    public override void Activate(int o)
    {
        throw new System.NotImplementedException();
    }
}
