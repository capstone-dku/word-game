using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAlarm : MonoBehaviour
{
    [SerializeField] private Text uiText;
    public void SetText(string text)
    {
        this.uiText.text = text;
    }
    public void OnClickedClose()
    {
        gameObject.SetActive(false);
    }
}
