using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelQuiz : MonoBehaviour
{
    private int currentIndex = 0;
    private List<Voca> vocaList = null;
    private int answerIndex = 0; // 정답 버튼의 인덱스
    private int answerCount = 0; // 사용자가 맞춘 정답 갯수
    [SerializeField] private Text textVoca;
    [SerializeField] private Text[] textButton;
    [SerializeField] private Image imageCorrect;
    [SerializeField] private Image imageWrong;
    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        this.currentIndex = 0;
    }

    public void StartQuiz()
    {
        // 단어 제시
        textVoca.text = vocaList[currentIndex].voca;
        // 4개의 버튼 중 하나의 버튼을 정답으로 만듬
        answerIndex = Random.Range(0, textButton.Length-1);
        textButton[answerIndex].text = vocaList[currentIndex].meaning[0];

        // 나머지 버튼을 랜덤한 오답으로 만듬
        List<int> random = new List<int>();
        for (int i = 0; i < textButton.Length; i++)
        {
            if (i == answerIndex) continue;
            // 학습한 단어 중 랜덤한 단어를 가져옴
            int rnd = Random.Range(0, vocaList.Count);
            while (random.Contains(rnd) && rnd == currentIndex)
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
            Debug.Log("정답");
            imageCorrect.gameObject.SetActive(true);
            answerCount++;
        }
        else
        {
            Debug.Log("오답");
            imageWrong.gameObject.SetActive(true);
        }

        if (currentIndex == vocaList.Count - 1)
        {
            Debug.Log("퀴즈 끝");
        }
        else
        {
            StartCoroutine(NextQuiz());
        }
    }

    IEnumerator NextQuiz()
    {
        yield return new WaitForSeconds(2.0f);
        imageCorrect.gameObject.SetActive(false);
        imageWrong.gameObject.SetActive(false);
        currentIndex++;
        StartQuiz();
        yield return null;
    }
}
