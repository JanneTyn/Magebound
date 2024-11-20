using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxMana(float maxMana)
    {
        slider.maxValue = maxMana;
    }

    public void SetCurrentMana(float currentMana)
    {
        slider.value = currentMana;
    }
}
