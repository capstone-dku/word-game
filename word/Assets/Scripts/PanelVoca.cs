using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PanelVoca : MonoBehaviour
{
    [SerializeField] private Text textVoca;
    [SerializeField] private Text textMeaning;
    [SerializeField] private Image imageBackground;
    [SerializeField] private StringBuilder sb = new StringBuilder();
    public void UpdatePanel(Voca voca, bool answer)
    {
        textVoca.text = voca.voca;
        sb.Clear();
        if (voca.meaning.Length >= 2)
        {
            for (int i = 0; i < voca.meaning.Length; i++)
            {
                sb.Append(voca.meaning[i]);
                sb.Append("\n");
            }
        }
        else
        {
            sb.Append(voca.meaning[0]);
        }
        textMeaning.text = sb.ToString();

        if (answer)
            imageBackground.color = Color.green;
        else
            imageBackground.color = Color.red;

    }
}
