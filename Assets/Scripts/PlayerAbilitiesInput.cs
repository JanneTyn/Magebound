using System.Collections;
using UnityEngine;

public class PlayerAbilitiesInput : MonoBehaviour
{

    private bool isGlobalCooldownActive = false;
    public float globalCooldownDuration = 2f;

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
        spellVortex = GetComponent<SpellVortex>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGlobalCooldownActive && Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<Ability_Wall>().ActivateAbility(GetComponent<CharacterStats>().GetCurrentElement());
            StartCoroutine(GlobalCooldown());
        }

        if (!isGlobalCooldownActive && Input.GetKeyDown(KeyCode.F))
        {
            spellVortex.StartTargeting();
        }

        if (!isGlobalCooldownActive && spellVortex != null  && spellVortex.IsTargetingActive() && Input.GetMouseButtonDown(0)) {
            spellVortex.ConfirmTarget();
            StartCoroutine(GlobalCooldown());
        }

        /*else if (!spellVortex.IsTargetingActive() && Input.GetMouseButtonDown(0))
        {
            //Players basic attack
        }*/
    }

    private void FixedUpdate()
    {

        //To Change Elements
        if (!isGlobalCooldownActive && Input.GetKey("1"))
        {
            GetComponent<CharacterStats_PlayerStats>().SetCurrentElement(1);
            uiSettings.skillIconsParent.GetComponent<SkillIconParent>().ChangeElement(1);
            StartCoroutine(GlobalCooldown());
        }
        if (!isGlobalCooldownActive && Input.GetKey("2"))
        {
            GetComponent<CharacterStats_PlayerStats>().SetCurrentElement(2);
            uiSettings.skillIconsParent.GetComponent<SkillIconParent>().ChangeElement(2);
            StartCoroutine(GlobalCooldown());
        }
        if (!isGlobalCooldownActive && Input.GetKey("3"))
        {
            GetComponent<CharacterStats_PlayerStats>().SetCurrentElement(3);
            uiSettings.skillIconsParent.GetComponent<SkillIconParent>().ChangeElement(3);
            StartCoroutine(GlobalCooldown());
        }

    }

    private IEnumerator GlobalCooldown()
    {
        isGlobalCooldownActive = true;
        uiSettings.skillIconsParent.GetComponent<SkillIconParent>().StartGlobalCooldown(globalCooldownDuration);
        yield return new WaitForSeconds(globalCooldownDuration);
        isGlobalCooldownActive = false;
    }
}
