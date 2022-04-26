using System;
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

    [SerializeField] private PanelStudyConfirm panelStudyConfirm; // 학습 확인 창

    [SerializeField] private GameObject panelStudy; // 깜빡이 학습 패널창
    [SerializeField] private Text textStudy; // 깜빡이 학습 단어 UI텍스트
    [SerializeField] private Text textVocaCount; // 깜빡이 학습 단어 갯수 UI텍스트
    [SerializeField] private Text textStudyCount; // 깜빡이 학습 회차 UI텍스트
    [SerializeField] private Button buttonStartQuiz;

    private List<Voca> vocaList;
    [SerializeField] private PanelQuiz panelQuiz;

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

    /// <summary>
    /// 단어 세트를 골랐을때 호출되는 함수.
    /// 깜빡이 학습 확인창을 보여준다.
    /// </summary>
    public void StudyConfirm(int difficulty, int setNumber, int star)
    {
        panelStudyConfirm.gameObject.SetActive(true);
        panelStudyConfirm.UpdateStars(star);
        panelStudyConfirm.UpdateSetNumber(setNumber);
    }

    /// <summary>
    /// 깜빡이 학습 확인창에서 확인버튼을 눌렀을 때 호출되는 함수,
    /// 깜빡이 학습을 시작한다.
    /// </summary>
    public void Study(int difficulty, int setNumber)
    {
        // TODO: vocaSelector 구현 후 함수 바꾸기
        //List<Voca> voca = vocaSelector.SelectVoca(difficulty, setNumber);
        
        // 테스트용 단어 세트
        // 테스트하고나서 지우기
        List<Voca> voca = new List<Voca>();
        voca.Add(new Voca(0, "A", "에이", 0));
        voca.Add(new Voca(1, "B", "비", 0));
        voca.Add(new Voca(2, "C", "씨", 0));
        
        panelStudy.SetActive(true);
        panelStudyConfirm.gameObject.SetActive(false);
        panelDifficulty.SetActive(false);
        panelVocaSet[difficulty].gameObject.SetActive(false);
        StartCoroutine(ShowVoca(voca));

    }

    IEnumerator ShowVoca(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        buttonStartQuiz.gameObject.SetActive(false);
        textStudy.text = "학습이 시작됩니다.";
        textStudyCount.text = "";
        textVocaCount.text = "";
        yield return new WaitForSeconds(2.0f);
        for (int i = 1; i <= 3; i++)
        {
            textStudyCount.text = i.ToString() + "회차";
            for (int j = 0; j < vocaList.Count; j++)
            {
                textVocaCount.text = (j + 1).ToString() + " / " + vocaList.Count.ToString();
                textStudy.text = vocaList[j].voca;
                yield return new WaitForSeconds(2.0f);
                textStudy.text = vocaList[j].meaning;
                yield return new WaitForSeconds(2.0f);
                textStudy.text = "";
                yield return null;
            }
        }

        textStudy.text = "학습이 종료되었습니다.";
        yield return new WaitForSeconds(2.0f);
        textStudy.text = "퀴즈가 시작됩니다.";
        
        buttonStartQuiz.gameObject.SetActive(true);
        yield return null;
        buttonStartQuiz.gameObject.SetActive(true);
    }
    
    public void Quiz()
    {
        buttonStartQuiz.gameObject.SetActive(false);
        panelStudy.gameObject.SetActive(false);
        panelQuiz.gameObject.SetActive(true);
        panelQuiz.Init(this.vocaList);
        panelQuiz.StartQuiz();
    }

    public void OnButtonClickQuiz(int num)
    {

    }
}
