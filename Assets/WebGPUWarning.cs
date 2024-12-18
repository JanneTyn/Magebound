using UnityEngine;

public class WebGPUWarning : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("Windows version active");
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void CloseButton()
    {
        instructionsPanel.SetActive(false);
    }

}
