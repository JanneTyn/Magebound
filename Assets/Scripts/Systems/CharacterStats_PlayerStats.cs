using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats_PlayerStats : CharacterStats
{
    public HealthBar healthBar;
    public GameOverFade gameOverScreen;
    public int points;
    public float damageMultiplier = 1.0f;
    public bool playerDead = false;

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
            StartDeathAnimation();
        }
    }

    public void LevelUp(float damageMultiplier, float maxHealth, float maxMana)
    {
        this.damageMultiplier += damageMultiplier;

        SetMaxHealth(GetMaxHealth() + maxHealth);
        SetCurrentHealth(GetMaxHealth());
        healthBar.SetCurrentHealth(GetCurrentHealth());

        GetComponent<ManaSystem>().LevelUp(maxMana);
    }

    private void StartDeathAnimation()
    {
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        GetComponentInChildren<Animator>().Play("Dead", 0);
        playerDead = true;

        yield return new WaitForEndOfFrame();

        while ((GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dead") &&
            GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
        {
            yield return null;
        }

        gameOverScreen.SetUI(true);

        Destroy(gameObject);
        Debug.Log("Player died.");
    }
}
