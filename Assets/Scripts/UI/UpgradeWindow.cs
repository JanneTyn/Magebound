using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private TextAsset upgrades;

    [System.Serializable]
    public class Upgrades
    {
        public int id;
        public string title;
        public string iconHeader;
        public Sprite icon;
        public string effectText;
        public float value;
    }

    [System.Serializable]
    public class UpgradeList
    {
        public Upgrades[] upgrades;
    }

    public UpgradeList upgradeList = new UpgradeList();
    public List<Upgrades> randomUpgrades = new List<Upgrades>();

    public GameObject[] toUpdate;
    private int upgradeIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgrades = Resources.Load<TextAsset>("upgradesJSON");

        upgradeList = JsonUtility.FromJson<UpgradeList>(upgrades.text);
        
        foreach(var upgrade in upgradeList.upgrades) 
        {
            upgrade.icon = LoadSpriteFromRseources(upgrade.iconHeader);
        }
    }
    
    private Sprite LoadSpriteFromRseources(string spriteName)
    {
        Sprite sprite = Resources.Load<Sprite>("Icons/" + spriteName);
        if(sprite == null)
        {
            Debug.LogWarning("No sprite of " + spriteName + " was found");
        }
        return sprite;
    }

    public void GetRandomUpgrades(int length)
    {
        randomUpgrades.Clear();
        int savedRandomIndex = 100;

        for(int i = 0; i < length; i++)
        {
            
            if (upgradeList.upgrades != null && upgradeList.upgrades.Length > 0)
            {
                int randomIndex = Random.Range(0, upgradeList.upgrades.Length);
                while (randomIndex == savedRandomIndex)
                {
                    randomIndex = Random.Range(0, upgradeList.upgrades.Length);
                }
                savedRandomIndex = randomIndex;
                randomUpgrades.Add(upgradeList.upgrades[randomIndex]);
            }
            else
            {
                Debug.LogWarning("UpgradeListIsEmpty or JSON not found");
            }
        }

    }

    public void OpenUpgradeWindow()
    {

        gameObject.SetActive(true);
        StartCoroutine(SomethingSequence());
        

    }

    private IEnumerator SomethingSequence()
    {
        yield return null;

        upgradeIndex = 0;
        GetRandomUpgrades(2);

        foreach (var upgrade in randomUpgrades)
        {
            toUpdate[upgradeIndex].GetComponent<UpgradeSlot>().UpdateWindow(upgrade.id, upgrade.title, upgrade.icon, upgrade.effectText, upgrade.value);
            upgradeIndex++;
        }

        Time.timeScale = 0;
    }
}
