using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeSlot : MonoBehaviour
{
    public int id;
    public TMP_Text title;
    public Image icon;
    public TMP_Text effect;
    public float value;

    GameObject[] childObjects;
    public GameObject upgradeWindow;

    private void Start()
    {
        childObjects = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
        }

        title = childObjects[0].GetComponent<TMP_Text>();
        icon = childObjects[1].GetComponent<Image>();
        effect = childObjects[2].GetComponent<TMP_Text>();
    }

    public void UpdateWindow(int id, string title, Sprite icon, string effect, float value)
    {
        this.id = id;
        this.title.text = title;
        this.icon.sprite = icon;
        this.effect.text = effect;
        this.value = value;
    }

    public void SendInfo()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<UpgradeManager>().ApplyUpgrade(id, value);
        Time.timeScale = 1;
        upgradeWindow.SetActive(false);
    }
}
