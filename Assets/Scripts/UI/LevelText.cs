using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelText : MonoBehaviour
{
    public TMP_Text text;

    public void setLevel(string level)
    {
        text.text = level;
    }
}
