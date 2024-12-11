using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private SpellBaseEffect spellEffect;
    private SpellExplosion spellExplosion;
    private SpellShard spellShard;
    private Vector3 fixedPoint;
    bool attackActive = false;
    private void Start()
    {
        spellEffect = GameObject.Find("SpellBaseEffect").GetComponent<SpellBaseEffect>();
        spellExplosion = GameObject.Find("SpellBaseEffect").GetComponent<SpellExplosion>();
        spellShard = GameObject.Find("SpellBaseEffect").GetComponent<SpellShard>();
    }
    public IEnumerator InitializeAttackAnimation(Vector3 targetLoc, int attackID)
    {
        fixedPoint = targetLoc;

        if (attackActive) yield break;

        // Adjust the target location to the player's level
        AdjustTargetLocation(ref targetLoc);

        // Initiate attack animation
        StartAttackAnimation(attackID);

        // Duration of the attack
        float attackTime = 1f;
        Debug.Log("attacktime: " + attackTime);

        // Perform rotation and attack initialization during the attack duration
        yield return RotateAndAttack(targetLoc, attackID, attackTime);

        // Reset animation state
        ResetAttackAnimation();
    }

    void AdjustTargetLocation(ref Vector3 targetLoc)
    {
        targetLoc.y = transform.position.y;
        transform.LookAt(targetLoc);
    }

    void StartAttackAnimation(int attackID)
    {
        var animator = GetComponentInChildren<Animator>();
        animator.SetLayerWeight(1, 1.0f);
        if (attackID == 0)
            animator.SetBool("BasicAttackShot", true);
        else
            animator.SetBool("SkillAttack", true);
    }

    IEnumerator RotateAndAttack(Vector3 targetLoc, int attackID, float attackTime)
    {
        float elapsedTime = 0f;
        bool attackInitialized = false;
        attackActive = true;

        while (elapsedTime < attackTime)
        {
            elapsedTime += Time.deltaTime;

            // Continuously rotate towards the target
            transform.LookAt(targetLoc);

            // Initialize the attack at the delay
            if (elapsedTime > 0.5f && !attackInitialized)
            {
                attackInitialized = true;
                InitializeAttack(attackID);
            }

            yield return null;
        }
    }

    void ResetAttackAnimation()
    {
        var animator = GetComponentInChildren<Animator>();
        animator.SetBool("BasicAttackShot", false);
        animator.SetBool("SkillAttack", false);
        animator.SetLayerWeight(1, 0f);
        attackActive = false;
    }

    void InitializeAttack(int attackID)
    {
        switch (attackID)
        {
            case 0: //projectile
                spellEffect.InitializeProjectile(transform.position, fixedPoint, GetComponent<CharacterStats_PlayerStats>().GetCurrentElement());
                break;
            case 1: //Dash
                fixedPoint.y = transform.position.y;
                GetComponent<Dash>().InitializeDash(transform.position, fixedPoint, GetComponent<CharacterStats_PlayerStats>().GetCurrentElement());
                break;
            case 2: //explosion
                spellExplosion.InitializeExplosion(transform.position, fixedPoint, GetComponent<CharacterStats_PlayerStats>().GetCurrentElement());
                break;
            case 3: //shard
                fixedPoint.y = 0.4f;
                spellShard.InitializeShard(transform.position, fixedPoint, GetComponent<CharacterStats_PlayerStats>().GetCurrentElement());
                break;
            case 4: //wall
                GetComponent<Ability_Wall>().InitializeWall(GetComponent<CharacterStats>().GetCurrentElement());
                break;
            case 5: //vortex
                GetComponent<SpellVortex>().ConfirmTarget();
                break;
        }
    }

}
