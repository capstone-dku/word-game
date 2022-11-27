using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// 단어 학습 인터페이스와 학습을 맡는 클래스
public class VocaStudy : MonoBehaviour
{
    [SerializeField] private SaveLoad saveLoad;
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField] private PanelItem panelItem;

    [SerializeField] private GameObject panelDifficulty; // 난이도 선택 패널창

    [SerializeField] private PanelVocaSet[] panelVocaSet = new PanelVocaSet[4]; // 세트 선택 패널창

    [SerializeField] private PanelStudyConfirm panelStudyConfirm; // 학습 확인 창

    [SerializeField] private GameObject panelStudy; // 깜빡이 학습 패널창
    [SerializeField] private Text textStudy; // 깜빡이 학습 단어 UI텍스트
    [SerializeField] private Text textVocaCount; // 깜빡이 학습 단어 갯수 UI텍스트
    [SerializeField] private Text textStudyCount; // 깜빡이 학습 회차 UI텍스트
    [SerializeField] private Button buttonStartQuiz; // 퀴즈 시작 버튼

    [SerializeField] private PanelQuiz panelQuiz; // 퀴즈 패널창

    [SerializeField] private PanelVocaList panelVocaList; // 보카 리스트 패널 창

    [SerializeField] private PanelResult panelResult; // 결과창

    // 테스트용인지 아닌지 확인하기 위한 변수
    public bool TEST_MODE;
    private WaitForSeconds DelayTime; // 테스트모드일경우 깜빡이 학습 대기시간이 줄어듬

    private List<Voca> currentVocaList;
    private int currentDifficulty;
    private int currentSetNumber;

    private void Start()
    {
        if (TEST_MODE)
            DelayTime = new WaitForSeconds(0.1f);
        else
            DelayTime = new WaitForSeconds(1.0f);

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

    public void OnClose()
    {
        panelDifficulty.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            panelVocaSet[i].gameObject.SetActive(false);
        }
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
        panelStudyConfirm.UpdateDifficulty(diff);
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
        // 학습할 단어를 불러온다.
        Debug.Log(difficulty);
        currentVocaList = vocaSelector.SelectVoca(difficulty, setNumber);
        this.currentDifficulty = difficulty;
        this.currentSetNumber = setNumber;

        panelStudy.SetActive(true);
        panelStudyConfirm.gameObject.SetActive(false);
        panelDifficulty.SetActive(false);
        panelVocaSet[difficulty].gameObject.SetActive(false);
        StartCoroutine(ShowVoca());

    }

    IEnumerator ShowVoca()
    {
        buttonStartQuiz.gameObject.SetActive(false);
        textStudy.text = "학습이 시작됩니다.";
        textStudyCount.text = "";
        textVocaCount.text = "";
        
        yield return DelayTime;
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i <= 1; i++)
        {
            textStudyCount.text = i.ToString() + "회차";
            for (int j = 0; j < currentVocaList.Count; j++)
            {
                textVocaCount.text = (j + 1).ToString() + " / " + currentVocaList.Count.ToString();
                textStudy.text = currentVocaList[j].voca;
                yield return DelayTime;
                sb.Clear();
                if (currentVocaList[j].meaning.Length >= 2)
                {
                    for (int k = 0; k < currentVocaList[j].meaning.Length; k++)
                    {
                        sb.Append(currentVocaList[j].meaning[k]);
                        sb.Append("\n");
                    }
                }
                else
                {
                    sb.Append(currentVocaList[j].meaning[0]);
                }
                textStudy.text = sb.ToString();
                yield return DelayTime;
                textStudy.text = "";
                yield return null;
            }
        }

        textStudy.text = "학습이 종료되었습니다.";
        yield return DelayTime;
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
        panelQuiz.Init(this.currentVocaList);
        panelQuiz.StartQuiz();
    }

    public void OnQuizFinished(int answerCount, bool[] answer)
    {
        panelVocaList.gameObject.SetActive(true);
        panelVocaList.Init(currentVocaList, answer);
        panelQuiz.gameObject.SetActive(false);

        
        // TODO: https://github.com/capstone-dku/word-game/issues/5
        // 1. 결과창을 보여준다.
        panelResult.gameObject.SetActive(true);
        // 저장되어있던 데이터와 별 갯수 비교
        int savedStars = saveLoad.GetStars(currentDifficulty, currentSetNumber);
        int stars = 0;
        // FIX? 정답갯수 하드코드
        // 획득한 별 계산
        if (answerCount >= 20)
            stars = 3;
        else if (answerCount >= 12)
            stars = 2;
        else if (answerCount >= 6)
            stars = 1;
        if (savedStars < stars)
        {
            panelResult.UpdateStars(stars);
            // 지급할 티켓 계산
            int ticket = stars - savedStars;
            // 2. 획득한 별에 따라 사용자에게 티켓을 지급한다.
            Debug.Log("티켓 획득: " + ticket + "개");
            saveLoad.AddTicket(ticket);
            panelItem.UpdateTicket(saveLoad.GetTicket());
            // 3. 획득한 별 갯수를 저장한다.
            saveLoad.SetStars(currentDifficulty, currentSetNumber, stars);
            Debug.Log("별 갯수 저장: " + stars + "개");
            // 4. 미션 카운트 증가시킨다.
            saveLoad.AddTodayStudyMission(1);
            saveLoad.AddWeekStudyMission(1);
            
        }

        // 학습한 단어 저장 및 세트 해금
        // 1. 학습한 단어를 저장한다.
        vocaSelector.AddVocaWeight(currentDifficulty, currentSetNumber);
        // 퀴즈 정답에 따라 가중치를 다르게 저장한다.
        int[] weights = new int[20];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = answer[i] == true ? 10 : 30;
        }

        vocaSelector.SaveVocaWeight(currentVocaList, weights);
        // 2.다음 세트를 해금한다.
        // (별 1개 이상일 경우 해금)
        bool clear = stars > 0;
        saveLoad.LockVocaSet(currentDifficulty, currentSetNumber+1, !clear);
        // 3. 해금된 정보를 저장한다.



        saveLoad.SaveData();

        // 세트선택 패널의 버튼의 별갯수와 해금정보를 업데이트한다.
        panelVocaSet[currentDifficulty].UpdateButtonStars(currentSetNumber);
        panelVocaSet[currentDifficulty].UpdateButtonLocked(currentSetNumber + 1);
    }

    private void StudyFinished()
    {
        currentVocaList = null;
        currentDifficulty = 0;
        currentSetNumber = 0;
    }

}
