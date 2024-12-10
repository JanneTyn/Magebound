using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject[] toDisable; //To Disable when Pause menu active and reactivate if resumin
    public GameObject pauseMenu; 

    private bool pauseMenuIsActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!pauseMenuIsActive)
            {
                Time.timeScale = 0; //Pause game
                pauseMenu.SetActive(true); 
                foreach (GameObject go in toDisable)
                {
                    go.SetActive(false);
                }
            }
            else
            {

                Time.timeScale = 1; //Continue Game
                foreach (GameObject go in toDisable)
                {
                    go.SetActive(true);
                }
                pauseMenu.SetActive(false);              
            }

        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1; //Continue game
        foreach (GameObject go in toDisable)
        {
            go.SetActive(true);
        }
        pauseMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitButton()
    {
        //EditorApplication.isPlaying = false; //Comment this line before building
        Application.Quit();

    }
}
