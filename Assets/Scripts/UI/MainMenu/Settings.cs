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

    public void MainVolumeSlider(Slider volume)
    {
        AudioManager.Instance.AudioVolume(volume.value);
    }

    public void PlayerVolumeSlider(Slider volume)
    {
        AudioManager.Instance.playerVolume = volume.value;
    }

    public void EffectVolumeSlider(Slider volume)
    {
        AudioManager.Instance.effectVolume = volume.value;
    }
}
