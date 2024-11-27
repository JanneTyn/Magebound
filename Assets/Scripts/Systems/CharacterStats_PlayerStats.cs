using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats_PlayerStats : CharacterStats
{
    public HealthBar healthBar;
    public int points;

    private void Start()
    {
        //Set Start values for UI
        healthBar.SetMaxHealth(GetMaxHealth());
        healthBar.SetCurrentHealth(GetCurrentHealth());
    }

    public void GainPoints(int points)
    {
        this.points += points;
    }

    public override void ApplyDamage(float damage)
    {
        SetCurrentHealth(GetCurrentHealth() - damage);

        //Update UI
        healthBar.SetCurrentHealth(GetCurrentHealth());

        if (GetCurrentHealth() <= 0)
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        //TODO:
        //Death animation

        Destroy(gameObject);
        Debug.Log("Player died.");

        //DELETE:
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        throw new System.NotImplementedException();
    }
}
