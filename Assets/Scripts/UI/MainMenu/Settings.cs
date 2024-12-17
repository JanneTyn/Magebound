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
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.AudioVolume(volume.value);
        }
    }

    public void PlayerVolumeSlider(Slider volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.playerVolume = volume.value;
        }

    }

    public void EffectVolumeSlider(Slider volume)
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.effectVolume = volume.value;
        }
    }
}
