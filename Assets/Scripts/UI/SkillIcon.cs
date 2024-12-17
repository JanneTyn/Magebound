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

    public IEnumerator Pressed()
    {
        Color originalColor = image.color;

        float h, s, v;
        Color.RGBToHSV(originalColor, out h, out s, out v);
        v *= 0.2f;

        image.color = Color.HSVToRGB(h, s, v);

        while(v < 1)
        {
            v += Time.deltaTime * 1;
            image.color = Color.HSVToRGB(h, s, v);
            yield return null;
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
