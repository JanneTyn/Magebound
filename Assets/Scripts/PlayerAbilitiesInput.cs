using System.Collections;
using UnityEngine;

public class PlayerAbilitiesInput : MonoBehaviour
{

    private bool isGlobalCooldownActive = false;
    public float globalCooldownDuration = 1.5f;
    public float dashCooldownDuration = 2f;
    public float wallManaCost = 200f;
    public float vortexManaCost = 200f;
    public float dashManaCost = 200f;
    public float explosionManaCost = 200f;
    public float shardManaCost = 200f;
    CursorTarget cursorTarget;
    ManaSystem manaSystem;

    //Seperate possible extra needed settings for UI
    [System.Serializable]
    public class UISettings
    {
        public GameObject skillIconsParent;
    }
    public UISettings uiSettings;

    private SpellVortex spellVortex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursorTarget = GameObject.Find("PlayerFollow/Camera Pivot/MainCamera").GetComponent<CursorTarget>();
        spellVortex = GetComponent<SpellVortex>();
        manaSystem = GetComponent<ManaSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<CharacterStats_PlayerStats>().playerDead)
        {
            if (wallManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && Input.GetKeyDown(KeyCode.Q)) //Wall
            {
                GetComponent<Ability_Wall>().ActivateAbility(GetComponent<CharacterStats>().GetCurrentElement());
                manaSystem.UseMana(wallManaCost);
                StartCoroutine(GlobalCooldown(globalCooldownDuration));
            }
            else if (vortexManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && Input.GetKeyDown(KeyCode.F)) //Vortex
            {
                spellVortex.StartTargeting();
            }
            else if (vortexManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && spellVortex != null && spellVortex.IsTargetingActive() && Input.GetMouseButtonDown(0))
            {
                spellVortex.PrepareAttackAnim();
                manaSystem.UseMana(vortexManaCost);
                StartCoroutine(GlobalCooldown(globalCooldownDuration));
            }
            else if (vortexManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && spellVortex != null && spellVortex.IsTargetingActive() && Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D))
            {
                spellVortex.CancelTargeting();
            }
            else if (!spellVortex.IsTargetingActive() && Input.GetMouseButtonDown(0)) //basic projectile
            {
                cursorTarget.AttackPrepare(0);
                //StartCoroutine(GlobalCooldown());
            }
            else if (dashManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && Input.GetMouseButtonDown(1)) //Dash
            {
                cursorTarget.AttackPrepare(1);
                manaSystem.UseMana(dashManaCost);
                StartCoroutine(GlobalCooldown(dashCooldownDuration));
            }
            else if (explosionManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && Input.GetKeyDown(KeyCode.E)) //Explosion
            {
                cursorTarget.AttackPrepare(2);
                manaSystem.UseMana(explosionManaCost);
                StartCoroutine(GlobalCooldown(globalCooldownDuration));
            }
            else if (shardManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && Input.GetKeyDown(KeyCode.R)) //Shard
            {
                cursorTarget.AttackPrepare(3);
                manaSystem.UseMana(shardManaCost);
                StartCoroutine(GlobalCooldown(globalCooldownDuration));
            }
        }
    }

    private void FixedUpdate()
    {

        //To Change Elements
        if (Input.GetKey("1")) ChangeElemenent(1, 1f); //Fire
        if (Input.GetKey("2")) ChangeElemenent(2, 1f); //Ice
        if (Input.GetKey("3")) ChangeElemenent(3, 1f); //Electric

    }

    private void ChangeElemenent(int elementID, float audioSwitchDuration)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SwitchAudio(elementID, audioSwitchDuration);
        }
        GetComponent<CharacterStats_PlayerStats>().SetCurrentElement(elementID);
        uiSettings.skillIconsParent.GetComponent<SkillIconParent>().ChangeElement(elementID);
    }

    private IEnumerator GlobalCooldown(float duration)
    {
        isGlobalCooldownActive = true;
        uiSettings.skillIconsParent.GetComponent<SkillIconParent>().StartGlobalCooldown(duration);
        yield return new WaitForSeconds(duration);
        isGlobalCooldownActive = false;
    }
}
