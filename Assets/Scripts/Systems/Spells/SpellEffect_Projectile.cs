using UnityEngine;

public class SpellEffect_Projectile : SpellEffect
{
    [SerializeField] private float lifetime;
    [SerializeField] private bool isExplosive;
    [SerializeField] private bool isOverCharged;
    [SerializeField] private float overChargeBurnDamage;
    [SerializeField] private float overChargeBurnDuration;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask explosionLayer;
    [SerializeField] private float projectileSpeed = 8;
    [SerializeField] private float travelDistance = 20;

    private void Start()
    {
        base.BaseStart();
        overChargeBurnDamage = GameObject.FindWithTag("Player").GetComponent<CharacterStats_PlayerStats>().damageMultiplier * overChargeBurnDamage;
    }

    public override void Activate(int o)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool GetIsExplosive() { return isExplosive; }
    public virtual bool GetIsOverCharged() { return isOverCharged; }
    public virtual float GetOverChargeBurnDamage() { return overChargeBurnDamage; }
    public virtual float GetOVerChargeBurnDuration() {  return overChargeBurnDuration; }
    public virtual float GetExplosionRadius() { return explosionRadius; }
    public virtual LayerMask GetExplosionLayer() { return explosionLayer; }
    public virtual float GetProjectileSpeed() { return projectileSpeed; }
    public virtual float GetTravelDistance() { return travelDistance; }

    public virtual void SetIsExplosive(bool isExplosive) { this.isExplosive = isExplosive; }
    public virtual void SetIsOverCharged(bool isOverCharged) { this.isOverCharged = isOverCharged; }
}
