using UnityEngine;

public class SpellEffect_Projectile : SpellEffect
{
    [SerializeField] private float lifetime;
    [SerializeField] private bool isExplosive;
    [SerializeField] private float explosionRadius;
    public override void CheckOverlap(int o)
    {
        throw new System.NotImplementedException();
    }

    public override void Activate(int o)
    {
        throw new System.NotImplementedException();
    }
}
