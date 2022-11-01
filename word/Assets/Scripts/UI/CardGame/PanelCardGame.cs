using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PanelCardGame : MonoBehaviour
{
    [SerializeField] private PanelItem panelItem;
    [SerializeField] private SaveLoad saveLoad;
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField] private Text textRemainTime;
    [SerializeField] public Button[] buttonCards;
    [SerializeField] private GameObject panelWrong;
    [SerializeField] private GameObject panelCorrect;

    private const int VOCA_NUM = 10; // 퍼즐판 안에 몇개의 단어가 들어갈지 
    private const int WIDTH = 4; // 퍼즐판의 크기 
    private const int HEIGHT = 5; // 퍼즐판의 크기 WIDTH*HEIGHT = VOCA_NUM*2

    private int[,] cardSet;

    private List<Voca> vocaList; // 퍼즐판에 출제될 단어 리스트
    //
    // 선택한 카드
    //
    private bool cardSelected = false;
    private int cardY = -1;
    private int cardX = -1;
    //
    private int currentTime = 60;
    private Coroutine cardGameCoroutine = null;
    private int correctCards = 0;
    private int[] vocaWeight;

    public void Init(List<Voca> vocaList)
    {
        //// TEST/////
        // vocaList = vocaSelector.FindVocaWeight(VOCA_NUM);
        //////////////
        this.vocaList = vocaList;
    }
    public void StartGame()
    {
        Clear();
        MakePuzzle();
        cardGameCoroutine = StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        while (currentTime > 0)
        {
            currentTime--;
            textRemainTime.text = (currentTime / 60).ToString() + " : " + (currentTime % 60).ToString();
            yield return new WaitForSeconds(1.0f);
        }

        if (currentTime <= 0)
        {
            Debug.Log("게임 종료");
            panelWrong.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            panelWrong.SetActive(false);
            gameObject.SetActive(false);

        }
        yield return null;
    }
    
    private IEnumerator GameFinished()
    {
        StopCoroutine(cardGameCoroutine);
        // 보상 지급
        saveLoad.AdjustCoin(1, 25);
        saveLoad.AdjustCoin(2, 50);
        panelItem.UpdateCoin(1, saveLoad.GetCoin(1));
        panelItem.UpdateCoin(2, saveLoad.GetCoin(2));
        // 정답, 오답 단어 가중치 변경
        // vocaSelector.SaveVocaWeight(vocaList, vocaWeight);
        // 데이터 저장
        saveLoad.SaveData();
        gameObject.SetActive(false);
        // 결과창
        // panelVocaList.gameObject.SetActive(true);
        // panelVocaList.Init(vocaList, answer);

        panelCorrect.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        panelCorrect.SetActive(false);
        gameObject.SetActive(false);
        yield return null;
    }
    private void OnClickedCard(int x, int y)
    {
        // Debug.Log("x:" + x + ", y:" + y);
        // Debug.Log(GetCardString(x, y));
        if (cardSelected)
        {
            if (cardX == x && cardY == y)
            {
                buttonCards[y * WIDTH + x].GetComponent<Image>().color = Color.white;
                cardSelected = false;
                cardX = -1;
                cardY = -1;
            }
            else
            {
                Debug.Log("Correct Check: " +CorrectCheck(x,y,cardX,cardY));
                if (CorrectCheck(x, y, cardX, cardY))
                {
                    buttonCards[y * WIDTH + x].GetComponent<Image>().color = Color.grey;
                    buttonCards[y * WIDTH + x].interactable = false;
                    buttonCards[cardY * WIDTH + cardX].GetComponent<Image>().color = Color.grey;
                    buttonCards[cardY * WIDTH + cardX].interactable = false;

                    cardSelected = false;
                    cardX = -1;
                    cardY = -1;
                    correctCards++;
                    if (correctCards >= VOCA_NUM)
                    {
                        StartCoroutine(GameFinished());
                    }
                }
                else
                {
                    buttonCards[cardY * WIDTH + cardX].GetComponent<Image>().color = Color.white;
                    cardSelected = false;
                    cardX = -1;
                    cardY = -1;
                }
            }
        }
        else
        {
            buttonCards[y * WIDTH + x].GetComponent<Image>().color = Color.red;
            cardSelected = true;
            cardY = y;
            cardX = x;
        }
    }
    
    public void Clear() // CardSet을 생성하고 초기화함
    {
        cardSet = new int[WIDTH,HEIGHT];
        for (int i=0;i<WIDTH;i++)
        {
            for(int j=0;j<HEIGHT;j++)
            {
                cardSet[i, j] = i+(j*WIDTH);
                buttonCards[j * WIDTH + i].GetComponent<Image>().color = Color.white;
                buttonCards[j * WIDTH + i].onClick.RemoveAllListeners();
                buttonCards[j * WIDTH + i].interactable = true;
            }
        }

        currentTime = 60;
        vocaWeight = new int[VOCA_NUM];
    }
    
    public void MakePuzzle() // 카드를 섞어서 CardSet에 넣어둠
    {
        for(int i=0; i<VOCA_NUM*2; i++)//섞기
        {
            int ran = (int)Random.Range(0,VOCA_NUM*2);
            int temp = cardSet[i%WIDTH,(int)(i/WIDTH)];

            cardSet[i%WIDTH,(int)(i/WIDTH)] = cardSet[ran%WIDTH,(int)(ran/WIDTH)];
            cardSet[ran%WIDTH,(int)(ran/WIDTH)] = temp;
        }


        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                buttonCards[j*WIDTH + i].GetComponentInChildren<Text>().text = GetCardString(i, j);
                int x = i;
                int y = j;
                buttonCards[j*WIDTH+i].onClick.AddListener(()=>OnClickedCard(x,y));
            }
        }
    }

    public bool CorrectCheck(int x1, int y1, int x2, int y2) // (x1, y1)과 (x2, y2)에 있는 카드가 같은 카드인지 여부를 반환한다.
    {
        Debug.Log("Correct Check\nx1: " + x1 + ", y1: " + y1 + "\nx2: " + x2 + ", y2: " + y2 + "\n" + cardSet[x1,y1] + ", " + cardSet[x2,y2]);
        if ((int)(cardSet[x1,y1]/2) == (int)(cardSet[x2,y2]/2)){
            return true;
        }
        return false;
    }

    public String GetCardString(int x, int y) // c번째 카드에 적혀있는 내용을 String 형식으로 반환함
    {
        int n = cardSet[x,y];
        if(n%2 == 0)
        {
            return vocaList[(int)(n/2)].voca;
        }
        else
        {
            return vocaList[(int)(n/2)].meaning[0]; // 첫번째 뜻만 반환하는데 나중에 수정
        }
        
    }
}
