using UnityEngine;

public class Instructions : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;

    public void CloseButton()
    {
        instructionsPanel.SetActive(false);
    }

    public void SettingsButton()
    {
        instructionsPanel.SetActive(true);
    }
}
