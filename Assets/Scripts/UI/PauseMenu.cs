using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject[] toDisable; //To Disable when Pause menu active and reactivate if resuming
    public GameObject pauseMenu; 
    public GameObject settingsMenu;

    private bool pauseMenuIsActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!pauseMenuIsActive)
            {
                if(AudioManager.Instance != null)
                {
                    AudioManager.Instance.GamePaused = true;
                }
                Time.timeScale = 0; //Pause game
                pauseMenu.SetActive(true); 
                foreach (GameObject go in toDisable)
                {
                    go.SetActive(false);
                }
            }
            else
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.GamePaused = false;
                }
                Time.timeScale = 1; //Continue Game
                foreach (GameObject go in toDisable)
                {
                    go.SetActive(true);
                }
                pauseMenu.SetActive(false);   
                if(settingsMenu.active)
                {
                    settingsMenu.SetActive(false);
                }

            }

        }
    }

    public void ResumeButton()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.GamePaused = false;
        }
        Time.timeScale = 1; //Continue game
        foreach (GameObject go in toDisable)
        {
            go.SetActive(true);
        }
        pauseMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        AudioManager.Instance.gameStarted = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitButton()
    {
        //EditorApplication.isPlaying = false; //Comment this line before building
        Application.Quit();

    }
}
