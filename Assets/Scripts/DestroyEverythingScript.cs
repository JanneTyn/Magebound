using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEverythingScript : MonoBehaviour
{
    
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Component[] components = child.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (component.GetType() != typeof(UnityEngine.VFX.VisualEffect) && component.GetType() != typeof(UnityEngine.Transform))
                {
                    Destroy(component);
                }
            }
        }
    }
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
