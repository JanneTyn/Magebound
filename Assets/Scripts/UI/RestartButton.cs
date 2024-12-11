using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour 
{
    public void RestartScene()
    {
        AudioManager.Instance.RestartAudio();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
