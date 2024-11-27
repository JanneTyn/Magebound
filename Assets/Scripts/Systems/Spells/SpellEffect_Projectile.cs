using UnityEngine;

public class SpellEffect_Projectile : SpellEffect
{
    [SerializeField] private float lifetime;
    [SerializeField] private bool isExplosive;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask explosionLayer;
    [SerializeField] private float projectileSpeed = 8;
    [SerializeField] private float travelDistance = 20;

    public override void Activate(int o)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool GetIsExplosive() { return isExplosive; }
    public virtual float GetExplosionRadius() { return explosionRadius; }
    public virtual LayerMask GetExplosionLayer() { return explosionLayer; }
    public virtual float GetProjectileSpeed() { return projectileSpeed; }
    public virtual float GetTravelDistance() { return travelDistance; }

    public virtual void SetisExplosive(bool isExplosive) { this.isExplosive = isExplosive; }
}
