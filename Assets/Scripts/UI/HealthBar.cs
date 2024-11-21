using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float maxHealth) 
    {
        slider.maxValue = maxHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        slider.value = currentHealth;
    }
}
