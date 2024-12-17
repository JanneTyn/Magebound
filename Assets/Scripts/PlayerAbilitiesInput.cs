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
    public Material fireClothes;
    public Material iceClothes;
    public Material electricClothes;
    public Material currentMat;

    [SerializeField] private PlayerAudioHandler playerAudioHandler;
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
                UIAbilityPressed(0);
                playerAudioHandler.PlayWall();
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
                UIAbilityPressed(2);
                StartCoroutine(GlobalCooldown(globalCooldownDuration));
            }
            else if (vortexManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && spellVortex != null && spellVortex.IsTargetingActive() && Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D))
            {
                spellVortex.CancelTargeting();
            }
            else if (!spellVortex.IsTargetingActive() && Input.GetMouseButtonDown(0)) //basic projectile
            {
                cursorTarget.AttackPrepare(0);
                playerAudioHandler.PlayBasicAttack();
                //StartCoroutine(GlobalCooldown());
            }
            else if (dashManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && Input.GetMouseButtonDown(1)) //Dash
            {
                playerAudioHandler.PlayDash();
                cursorTarget.AttackPrepare(1);
                manaSystem.UseMana(dashManaCost);
                StartCoroutine(GlobalCooldown(dashCooldownDuration));
            }
            else if (explosionManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && Input.GetKeyDown(KeyCode.E)) //Explosion
            {
                cursorTarget.AttackPrepare(2);
                manaSystem.UseMana(explosionManaCost);
                playerAudioHandler.PlayExplosion();
                UIAbilityPressed(1);
                StartCoroutine(GlobalCooldown(globalCooldownDuration));
            }
            else if (shardManaCost <= manaSystem.GetMana() && !isGlobalCooldownActive && Input.GetKeyDown(KeyCode.R)) //CrystalShard
            {
                cursorTarget.AttackPrepare(3);
                manaSystem.UseMana(shardManaCost);
                playerAudioHandler.PlayCrystalShard();
                UIAbilityPressed(3);
                StartCoroutine(GlobalCooldown(globalCooldownDuration));
            }//Time for massive if not enough mana else if
            else if (Input.GetKeyDown(KeyCode.Q) && wallManaCost > manaSystem.GetMana())
            {
                playerAudioHandler.PlayNotEnoughMana();
            }
            else if (Input.GetKeyDown(KeyCode.F) && vortexManaCost > manaSystem.GetMana())
            {
                playerAudioHandler.PlayNotEnoughMana();
            }
            else if (Input.GetKeyDown(KeyCode.E) && explosionManaCost > manaSystem.GetMana())
            {
                playerAudioHandler.PlayNotEnoughMana();
            }
            else if (Input.GetKeyDown(KeyCode.R) && shardManaCost > manaSystem.GetMana())
            {
                playerAudioHandler.PlayNotEnoughMana();
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
        ChangeClothColor(elementID);
    }

    private void UIAbilityPressed(int id)
    {
        switch(id)
        {
            case 0: //Wall
                StartCoroutine(uiSettings.skillIconsParent.GetComponent<SkillIconParent>().skillIcons[0].GetComponent<SkillIcon>().Pressed());
                break;
            case 1: //Explosion
                StartCoroutine(uiSettings.skillIconsParent.GetComponent<SkillIconParent>().skillIcons[1].GetComponent<SkillIcon>().Pressed());
                break;
            case 2: //Vortex
                StartCoroutine(uiSettings.skillIconsParent.GetComponent<SkillIconParent>().skillIcons[2].GetComponent<SkillIcon>().Pressed());
                break;
            case 3: //Crystal Shard
                StartCoroutine(uiSettings.skillIconsParent.GetComponent<SkillIconParent>().skillIcons[3].GetComponent<SkillIcon>().Pressed());
                break;
        }
    }

    private IEnumerator GlobalCooldown(float duration)
    {
        isGlobalCooldownActive = true;
        uiSettings.skillIconsParent.GetComponent<SkillIconParent>().StartGlobalCooldown(duration);
        yield return new WaitForSeconds(duration);
        isGlobalCooldownActive = false;
    }

    void ChangeClothColor(int elementID)
    {
        switch (elementID) 
        {
            case 1:
                currentMat = fireClothes;
                break;
            case 2:
                currentMat = iceClothes;
                break;
            case 3:
                currentMat = electricClothes;
                break;
        }

        Material[] mats = transform.Find("MB Player Unity V1/P_RT_HEAD.001").GetComponent<SkinnedMeshRenderer>().materials;
        mats[2] = currentMat;
        transform.Find("MB Player Unity V1/P_RT_HEAD.001").GetComponent<SkinnedMeshRenderer>().materials = mats;
    }
}
