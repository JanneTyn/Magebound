using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEverythingScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyEverything());
    }

    private IEnumerator DestroyEverything()
    {
        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
    }
}
