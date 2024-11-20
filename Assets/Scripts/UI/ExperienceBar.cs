using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider slider;

    public void SetExperienceNeeded(float experienceNeeded)
    {
        slider.maxValue = experienceNeeded;
    }

    public void SetCurrentExperience(float currentExperience)
    {
        slider.value = currentExperience;
    }  

}
