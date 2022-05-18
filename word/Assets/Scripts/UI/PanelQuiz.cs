using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PanelQuiz : MonoBehaviour
{
    private int currentIndex = 0;
    private List<Voca> vocaList = null;
    private int answerIndex = 0; // 정답 버튼의 인덱스
    private int answerCount = 0; // 사용자가 맞춘 정답 갯수
    private bool[] answer = new bool[20];
    [SerializeField] private Text textVoca;
    [SerializeField] private Text textQuizCount;
    [SerializeField] private Text[] textButton;
    [SerializeField] private Image imageCorrect;
    [SerializeField] private Image imageWrong;

    [SerializeField] private VocaStudy vocaStudy;
    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        this.currentIndex = 0;
        for (int i = 0; i < answer.Length; i++)
            answer[i] = false;
    }

    public void StartQuiz()
    {
        textQuizCount.text = (currentIndex + 1).ToString() + " / 20";
        // 단어 선택
        Voca quiz = vocaList[currentIndex];
        // 단어 제시
        textVoca.text = quiz.voca;
        // 4개의 버튼 중 하나의 버튼을 정답으로 만듬
        answerIndex = Random.Range(0, textButton.Length);
        textButton[answerIndex].text = quiz.meaning[0];
        // 4개 버튼을 랜덤한 오답으로 만듬
        List<int> random = new List<int>();
        for (int i = 0; i < textButton.Length; i++)
        {
            if (i == answerIndex) continue;
            // 학습한 단어 중 랜덤한 단어를 가져옴
            int rnd = Random.Range(0, vocaList.Count);
            // 중복일 경우 다시 뽑음
            while (random.Contains(rnd) || rnd == currentIndex)
            {
                rnd = Random.Range(0, vocaList.Count);
            }
            random.Add(rnd);
            textButton[i].text = vocaList[rnd].meaning[0];
        }
    }

    public void OnButtonClickAnswer(int idx)
    {
        if (idx == answerIndex)
        {
            imageCorrect.gameObject.SetActive(true);
            answerCount++;
            answer[currentIndex] = true;
        }
        else
        {
            imageWrong.gameObject.SetActive(true);
            answer[currentIndex] = false;
        }

        if (currentIndex >= vocaList.Count - 1)
        {
            vocaStudy.OnQuizFinished(answerCount, answer);
        }
        else
        {
            StartCoroutine(NextQuiz());
        }
    }

    IEnumerator NextQuiz()
    {
        yield return new WaitForSeconds(0.5f);
        imageCorrect.gameObject.SetActive(false);
        imageWrong.gameObject.SetActive(false);
        currentIndex++;
        StartQuiz();
        yield return null;
    }
}
