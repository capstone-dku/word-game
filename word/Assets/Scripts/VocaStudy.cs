using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 단어 학습 인터페이스와 학습을 맡는 클래스
public class VocaStudy : MonoBehaviour
{
    [SerializeField] private SaveLoad saveLoad;
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField] private GameObject panelDifficulty; // 난이도 선택 패널창
    [SerializeField] private PanelVocaSet[] panelVocaSet = new PanelVocaSet[4]; // 세트 선택 패널창


    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            InitPanelVocaSet(i);
        }
    }
    
    /// <summary>
    /// 학습 버튼 눌렀을 때 호출되는 함수.
    /// 난이도 선택창을 연다.
    /// </summary>
    public void OnButtonClickedStudy()
    {
        // 난이도 선택창을 보여준다.
        panelDifficulty.SetActive(true);
        
    }
    /// <summary>
    /// 닫기 버튼 눌렀을 때 호출되는 함수.
    /// 패널 창을 닫는다.
    /// </summary>
    /// <param name="panel">닫을 패널 창</param>
    public void OnButtonClickedClose(GameObject panel)
    {
        panel.SetActive(false);
    }

    /// <summary>
    /// 초,중,고,토익 버튼 눌렀을 때 호출되는 함수. 단어 세트 선택 창을 연다.
    /// </summary>
    /// <param name="diff">난이도. 0: 초등, 1: 중등, 2: 고등, 3: 토익</param>
    public void OnButtonClickDifficulty(int diff)
    {
        ShowPanelVocaSet(diff);
    }

    /// <summary>
    /// 해당 난이도의 세트선택 패널창을 보여준다.
    /// </summary>
    /// <param name="diff"></param>
    public void ShowPanelVocaSet(int diff)
    {
        panelVocaSet[diff].gameObject.SetActive(true);
    }

    /// <summary>
    /// 해당 난이도의 세트선택 패널창을 초기화한다. 
    /// </summary>
    /// <param name="diff"></param>
    private void InitPanelVocaSet(int diff)
    {
        panelVocaSet[diff].InitPanel();
    }
}
