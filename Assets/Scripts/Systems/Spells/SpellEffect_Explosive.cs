using UnityEngine;

public class SpellEffect_Explosive : SpellEffect
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private bool isExplosive;
    [SerializeField] private bool isOverCharged;
    [SerializeField] private LayerMask explosionLayer;

    [SerializeField] private float boltSpeed = 5;
    [SerializeField] private float statusDuration = 3;

    public virtual float GetBoltSpeed() { return boltSpeed; }
    public virtual float GetStatusDuration() { return statusDuration; }
    public virtual bool GetIsExplosive() { return isExplosive; }
    public virtual bool GetIsOverCharged() { return isOverCharged; }
    public virtual float GetExplosionRadius() { return explosionRadius; }
    public virtual LayerMask GetExplosionLayer() { return explosionLayer; }
    public override void Activate(int o)
    {
        throw new System.NotImplementedException();
    }
    
}
