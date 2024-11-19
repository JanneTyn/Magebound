using UnityEngine;

public class SpellEffect_Projectile : SpellEffect
{
    [SerializeField] private float lifetime;
    [SerializeField] private bool isExplosive;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask explosionLayer;
    public override void CheckOverlap(int o)
    {
        throw new System.NotImplementedException();
    }

    public override void Activate(int o)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool GetIsExplosive() { return isExplosive; }

    public virtual float GetExplosionRadius() { return explosionRadius; }

    public virtual LayerMask GetExplosionLayer() { return explosionLayer; }
}
