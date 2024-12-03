using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats_EnemyStats : CharacterStats
{
    public int scoreWorth = 200;

    public override void ApplyDamage(float damage)
    {
        SetCurrentHealth(GetCurrentHealth() - damage);

        if (GetCurrentHealth() <= 0)
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        GameManager.Instance.increaseScore(scoreWorth); //Keep this at the top of sequence unless something needs to happen before score update

        Destroy(gameObject);
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }

        Debug.Log("Enemy died!");
        
    }
}
