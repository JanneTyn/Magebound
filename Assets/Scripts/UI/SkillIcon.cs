using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    public Slider slider;
    public Image image;
    private float h, s ,v;


    public void ChangeImageColor(int elementID)
    {
        switch (elementID)
        {
            case 01:
                if (ColorUtility.TryParseHtmlString("#FF6500", out Color OrangeColor))
                {
                    image.color = OrangeColor;
                    Color.RGBToHSV(OrangeColor, out h, out s, out v);
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
                    Color.RGBToHSV(BlueColor, out h, out s, out v);
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
                    Color.RGBToHSV(PurpleColor, out h, out s, out v);
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

        float tempV = v;
        Color.RGBToHSV(originalColor, out h, out s, out tempV);
        tempV *= 0.2f;

        image.color = Color.HSVToRGB(h, s, tempV);

        while(tempV < 1)
        {
            tempV += Time.deltaTime * 1;
            image.color = Color.HSVToRGB(h, s, tempV);
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
