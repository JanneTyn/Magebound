using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEverythingScript : MonoBehaviour
{
    private void Awake()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.effectVolume = 0f;
        }
    }
    private void Start()
    {
        
        StartCoroutine(DestroyEverything());
    }

    private IEnumerator DestroyEverything()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.effectVolume = 1f;
        Destroy(gameObject);
    }
}
