using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 단어 학습 인터페이스와 학습을 맡는 클래스
public class VocaStudy : MonoBehaviour
{
    [SerializeField]
    private VocaSelector vocaSelector; 
    [SerializeField]
    private GameObject panelDifficulty; // 난이도 선택 패널창
    [SerializeField]
    private GameObject[] panelVocaSet = new GameObject[4]; // 세트 선택 패널창

    /// <summary>
    /// 학습 버튼 눌렀을 때 호출되는 함수.
    /// 난이도 선택창을 연다.
    /// </summary>
    private void ShowPanelDifficulty()
    {
        // 난이도 선택창을 보여준다.
        panelDifficulty.SetActive(true);
        
    }
    /// <summary>
    /// 닫기 버튼 눌렀을 때 호출되는 함수
    /// 패널 창을 닫는다.
    /// </summary>
    /// <param name="go">닫을 패널 창</param>
    public void OnButtonClickedClose(GameObject go)
    {
        go.SetActive(false);
    }

    /// <summary>
    /// 초,중,고,토익 버튼 눌렀을 때 호출되는 함수. 단어 세트 선택 창을 연다.
    /// </summary>
    /// <param name="diff">0: 초등, 1: 중등, 2: 고등, 3: 토익</param>
    public void OnButtonClickDifficulty(int diff)
    {
        ShowPanelVocaSet(diff);
    }

    public void ShowPanelVocaSet(int i)
    {
        panelVocaSet[i].SetActive(true);
    }
}
