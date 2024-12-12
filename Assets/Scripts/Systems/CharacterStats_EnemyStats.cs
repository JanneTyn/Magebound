using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats_EnemyStats : CharacterStats
{
    public int scoreWorth = 200;

    [SerializeField] private float healthMultiplier = 0.2f;
    [SerializeField] private float damageMultiplier = 0.5f;

    private EnemyDeathHandler deathHandler;

    private void Start()
    {
        float healthScalingFactor = 1 + (GameManager.Instance.totalScore / (scoreWorth * 10f)) * healthMultiplier;
        SetMaxHealth(GetMaxHealth() * healthScalingFactor);
        SetCurrentHealth(GetMaxHealth());

        float damageScalingFactor = 1 + Mathf.Sqrt(GameManager.Instance.totalScore / (scoreWorth * 10f)) * damageMultiplier;
        GetComponent<EnemyAttack>().damage *= damageScalingFactor;

        deathHandler = GetComponent<EnemyDeathHandler>();
    }

    public override void ApplyDamage(float damage)
    {
        SetCurrentHealth(GetCurrentHealth() - damage);

        //Trigger the damage flash effect
        DamageFlash damageFlash = GetComponent<DamageFlash>();
        if (damageFlash != null) {
            StartCoroutine(damageFlash.EFlash());
        }
        else
        {
            Debug.LogWarning("CharacterStats_EnemyStats.cs: No DamageFlash component found");
        }

        if (GetCurrentHealth() <= 0)
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        GameManager.Instance.increaseScore(scoreWorth); //Keep this at the top of sequence unless something needs to happen before score update
        GameManager.Instance.RemoveEnemy(gameObject);

        if (deathHandler != null)
        {
            deathHandler.HandleDeath();
        }
    }
}
