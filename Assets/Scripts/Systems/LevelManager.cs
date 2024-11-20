using System.Buffers.Text;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int level = 1; //Current Level
    [SerializeField] private int experienceNeeded = 50; //Experience needed for next level
    [SerializeField] private int currentExperience = 0; //Accumulated xp

    public ExperienceBar experienceBar;
    public LevelText levelText;

    // Variables for the XP formula
    [SerializeField] private int baseXP = 50; // Base XP for level 1
    [SerializeField] private int increment = 20; // Linear increase in XP
    [SerializeField] private int multiplier = 5; // Exponential growth multiplier
    [SerializeField] private float exponent = 1.2f; // Exponential factor (k)

    private void Start()
    {
        experienceNeeded = CalculateExperienceNeeded(level);

        experienceBar.SetExperienceNeeded(experienceNeeded);
        experienceBar.SetCurrentExperience(currentExperience);
        levelText.setLevel(level.ToString());
    }

    public void GainExperience(int experienceAmmount)
    {
        currentExperience += experienceAmmount;
        Debug.Log("Gained " +  experienceAmmount + " of experience");

        if (currentExperience >= experienceNeeded) { LevelUp(); }

        experienceBar.SetCurrentExperience(currentExperience);
    }

    private void LevelUp()
    {
        currentExperience -= experienceNeeded;
        level++;
        
        levelText.setLevel(level.ToString());

        experienceNeeded = CalculateExperienceNeeded(level);

        experienceBar.SetExperienceNeeded(experienceNeeded);
    }

    private int CalculateExperienceNeeded(int level)
    {
        // Formula: XPbase + Increment * n + Multiplier * n^k
        return Mathf.RoundToInt(baseXP + increment * level + multiplier * Mathf.Pow(level, exponent));
    }

}
