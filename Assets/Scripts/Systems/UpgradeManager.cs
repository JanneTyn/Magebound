using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private float damageMultiplier = 1;
    [SerializeField] private float speedMultiplier = 1;
    [SerializeField] private float sizeMultiplier = 1;

    public float GetDamageMultiplier() { return damageMultiplier; }
    public float GetSpeedMultiplier() { return speedMultiplier; }
    public float GetSizeMultiplier() {  return sizeMultiplier; }


    public void ApplyUpgrade(int id, float value)
    {
        switch (id)
        {
            case 1:
                damageMultiplier += value / 100f;
                break;
            case 2:
                speedMultiplier += value / 100f;
                break;
            case 3:
                sizeMultiplier += value / 100f;
                break;
        }
    }
}
