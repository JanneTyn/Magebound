using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject[] toEnable;
    bool enableDisableIsActive;

    public void StartButton()
    {
        AudioManager.Instance.StartGame();
        AudioManager.Instance.RestartAudio();
        SceneManager.LoadScene("SampleScene");        
    }

    public void SupriseButton()
    {
        Application.OpenURL("https://shattereddisk.github.io/rickroll/rickroll.mp4");
    }

    public void SettingsButton()
    {

    }

    public void QuitButton()
    {
        if(!enableDisableIsActive)
        {
            StartCoroutine(EnableThenDisable());
        }
    }

    IEnumerator EnableThenDisable()
    {
        enableDisableIsActive = true;

        foreach (var go in toEnable)
        {
            go.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (var go in toEnable)
        {
            go.SetActive(false);
        }

        enableDisableIsActive = false;
    }
}
