using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCrossWord : MonoBehaviour
{
    [SerializeField] private GameObject panelKeyboard;
    private List<Voca> vocaList; // 퍼즐판에 출제될 단어 리스트
    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
    }
    public void StartGame()
    {

    }

    private void ShowKeyboard(bool show)
    {
        panelKeyboard.SetActive(show);
    }

    public void OnClickedKeyboard(GameObject button)
    {
        Debug.Log(button.name);
    }

}
