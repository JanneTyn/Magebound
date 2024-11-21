using UnityEditor;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject[] toDisable;
    public GameObject pauseMenu;

    private bool pauseMenuIsActive = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!pauseMenuIsActive)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                foreach (GameObject go in toDisable)
                {
                    go.SetActive(false);
                }
            }
            else
            {
                Time.timeScale = 1;
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
        Time.timeScale = 1;
        foreach (GameObject go in toDisable)
        {
            go.SetActive(true);
        }
        pauseMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        //Implement shift to MainMenu when made
    }

    public void ExitButton()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();

    }
}
