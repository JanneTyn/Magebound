using UnityEngine;

public class SpellEffect_WorldEffect : SpellEffect
{
    [SerializeField] private float duration;

    public override void Activate(int o)
    {
        throw new System.NotImplementedException();
    }

    public virtual float GetDuration() { return duration; }
    public virtual void SetDuration(int o) { duration = o; }
}
