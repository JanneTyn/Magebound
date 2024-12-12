using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    public void CloseButton()
    {
        settingsPanel.SetActive(false);
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);
    }

    public void VolumeSlider(Slider volume)
    {
        AudioManager.Instance.AudioVolume(volume.value);
    }
}
