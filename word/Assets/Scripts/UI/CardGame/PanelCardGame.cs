using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.XPath;
using Unity.Notifications.iOS;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PanelCardGame : MonoBehaviour
{
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField] public Button[] buttonCards;

    private const int VOCA_NUM = 10; // 퍼즐판 안에 몇개의 단어가 들어갈지 
    private const int WIDTH = 4; // 퍼즐판의 크기 
    private const int HEIGHT = 5; // 퍼즐판의 크기 WIDTH*HEIGHT = VOCA_NUM*2

    private int[,] cardSet;
    private List<Voca> vocaList; // 퍼즐판에 출제될 단어 리스트

    public void Init(List<Voca> vocaList)
    {
        //// TEST/////
        vocaList = vocaSelector.FindVocaWeight(VOCA_NUM);
        //////////////
        this.vocaList = vocaList;
    }
    public void StartGame()
    {

    }

    void Start()
    {
        //// TEST/////
        Init(null);
        //////////////
        Clear();
        MakePuzzle();
    }
    
    public void Clear() // CardSet을 생성하고 초기화함
    {
        cardSet = new int[WIDTH,HEIGHT];
        for (int i=0;i<WIDTH;i++)
        {
            for(int j=0;j<HEIGHT;j++)
            {
                cardSet[i, j] = i+(j*WIDTH);
            }
        }
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
                buttonCards[i * WIDTH + j].GetComponentInChildren<Text>().text = GetCardString(i, j);
            }
        }
    }

    public bool CorrectCheck(int x1, int y1, int x2, int y2) // (x1, y1)과 (x2, y2)에 있는 카드가 같은 카드인지 여부를 반환한다.
    {
        if((int)(cardSet[x1,y1]/2) == (int)(cardSet[x2,y2])){
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
