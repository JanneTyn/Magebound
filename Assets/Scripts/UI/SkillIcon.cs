using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    public Slider slider;
    public Image image;

    public void ChangeImageColor(int elementID)
    {
        switch (elementID)
        {
            case 01:
                if (ColorUtility.TryParseHtmlString("#FF6500", out Color OrangeColor))
                {
                    image.color = OrangeColor;
                }
                else
                {
                    Debug.Log("Failed to Parse OrangeColor");
                }
                break;
            case 02:
                if (ColorUtility.TryParseHtmlString("#10CBFF", out Color BlueColor))
                {
                    image.color = BlueColor;
                }
                else
                {
                    Debug.Log("Failed to Parse BlueColor");
                }
                break;
            case 03:
                if (ColorUtility.TryParseHtmlString("#8D10FF", out Color PurpleColor))
                {
                    image.color = PurpleColor;
                }
                else
                {
                    Debug.Log("Failed to Parse PurpleColor");
                }
                break;
        }          
        
    }

    public IEnumerator CoolDown(float duration)
    {
        slider.value = 0;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            slider.value = Mathf.Clamp01(elapsedTime / duration);

            yield return null;
        }

        slider.value = 1;
    }
}
