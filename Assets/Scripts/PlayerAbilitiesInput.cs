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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        //To Change Elements
        if (!isGlobalCooldownActive && Input.GetKey("1"))
        {
            GetComponent<CharacterStats_PlayerStats>().SetCurrentElement(1);
            uiSettings.skillIconsParent.GetComponent<SkillIconParent>().ChangeElement(1);
            StartCoroutine(GlobalCooldown(globalCooldownDuration));
        }
        if (!isGlobalCooldownActive && Input.GetKey("2"))
        {
            GetComponent<CharacterStats_PlayerStats>().SetCurrentElement(2);
            uiSettings.skillIconsParent.GetComponent<SkillIconParent>().ChangeElement(2);
            StartCoroutine(GlobalCooldown(globalCooldownDuration));
        }
        if (!isGlobalCooldownActive && Input.GetKey("3"))
        {
            GetComponent<CharacterStats_PlayerStats>().SetCurrentElement(3);
            uiSettings.skillIconsParent.GetComponent<SkillIconParent>().ChangeElement(3);
            StartCoroutine(GlobalCooldown(globalCooldownDuration));
        }
    }

    private IEnumerator GlobalCooldown(float duration)
    {
        isGlobalCooldownActive = true;
        uiSettings.skillIconsParent.GetComponent<SkillIconParent>().StartGlobalCooldown(duration);
        yield return new WaitForSeconds(duration);
        isGlobalCooldownActive = false;
    }
}
