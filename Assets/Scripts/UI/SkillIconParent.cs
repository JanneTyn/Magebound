using UnityEngine;

public class SkillIconParent : MonoBehaviour
{
    public GameObject[] skillIcons;

    private void Start()
    {
        ChangeElement(1);   
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartGlobalCooldown(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeElement(1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)) 
        { 
            ChangeElement(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        { 
            ChangeElement(3); 
        }
        */
    }

    public void ChangeElement(int elementID)
    {
        foreach (var skillIcon in skillIcons)
        {
            skillIcon.GetComponent<SkillIcon>().ChangeImageColor(elementID);
        }
    }

    public void StartGlobalCooldown(float cooldownDuration)
    {
        SkillIcon _skillIcon;
        foreach (var skillIcon in skillIcons)
        {
            _skillIcon = skillIcon.GetComponent<SkillIcon>();
            StartCoroutine(_skillIcon.CoolDown(cooldownDuration));
        }
    }
}
