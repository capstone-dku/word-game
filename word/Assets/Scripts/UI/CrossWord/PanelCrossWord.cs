using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ALPHABET
{
    a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,Length, Empty
}

public class PanelCrossWord : MonoBehaviour
{
    [SerializeField] private GameObject panelKeyboard;
    private const int VOCA_NUM = 5; // 퍼즐판 안에 몇개의 단어가 들어갈지
    private const int WIDTH = 15; // 퍼즐판의 크기
    private const int HEIGHT = 15; // 퍼즐판의 크기

    private List<Voca> vocaList; // 퍼즐판에 출제될 단어 리스트

    public char[] wordPuzzle; // 퍼즐판. 0의 경우 비어 있음 이외의 경우 채워야하는 답으로 문자형태로 들어가있음.
    public int complete = 0;

    public bool Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        
        Clear();
    }

    public void Clear()
    {
        for(int i=0;i<WIDTH;i++){
            for(int j=0;j>HEIGHT;j++){
                wordPuzzle[i][j]=0;
            }
        }
        currentTime = time;
        private bool running = false;

        complete = false;
        running = false;
        UpdateSprites();
    }

    private void MakePuzzle(List<Voca> vocaList)
    {
        for(int i=0 ; i<VOCA_NUM ; i++)//긴 단어 순으로 word 정렬
        {
            int temp = i;
            int tempLength = vocaList[i].voca.length;
            for(int j=i+1 ; j<VOCA_NUM ; j++)
            {
                if(vocaList[j].voca.length > tempLength )
                {
                    temp = j;
                    tempLength = vocaList[j].voca.length;
                }
                Voca tempVoca = vocaList[i];
                vocaList[i] = vocaList[j];
                vocaList[j] = tempVoca;
            }
        }

        for(int i=0 ; i<VOCA_NUM ; i++)
        {
            int length = vocaList[i].voca.length;
            if(i==0)//첫 단어
            {
                if(Random.Range(0,2)<1){//가로 
                    x = Random.Range(0, WIDTH-length);
                    y = Random.Range(0, HEIGHT); 

                    y = Random.Range(0, HEIGHT-length);
                    x = Random.Range(0, WIDTH-length);
                    for(int j=0;j<length;j++){
                        wordPuzzle[x+j][y] = vocaList[i].voca[j];
                    }
                }
                else{//세로
                    x = Random.Range(0, WIDTH);
                    y = Random.Range(0, HEIGHT-length);

                    y = Random.Range(0, HEIGHT-length);
                    x = Random.Range(0, WIDTH-length);
                    for(int j=0;j<length;j++){
                        wordPuzzle[x][y+j] = vocaList[i].voca[j];
                    }
                }
            }
            else{//두번 째 부터
                for(int count = 0; count < 100 ; count++){//겹치는 부분 확인 count 횟수 만큼 검색 << 나중에 리팩토링
                    x = Random.Range(0, WIDTH);
                    y = Random.Range(0, HEIGHT);

                    for(int j=0; j<length;j++){
                        if(wordPuzzle[x][y] == vocaList[i].voca[j]){
                            int checkVaild = 0;
                            int checkRow = 0;
                            int checkCol = 0;
                            if(x-1 >= 0){
                                if(wordPuzzle[x-1][y] != 0){
                                    checkRow = 1;
                                }
                            }
                            if(x+1 < WIDTH){
                                if(wordPuzzle[x+1][y] != 0){
                                    checkRow = 1;
                                }
                            }
                            if(y-1 >=0 0){
                                if(wordPuzzle[x][y-1] != 0){
                                    checkCol = 1;
                                }
                            }
                            if(y+1 < HEIGHT){
                                if(wordPuzzle[x][y+1] != 0){
                                    checkCol = 1;
                                }
                            }
                            if(checkRow == 0){
                                if((k-j>0) and (k+length-1<WIDTH)){
                                for(int k=0;k<length;k++){
                                    
                                }
                            }
                            else if(checkCol == 0){
                                if((k-j>0) and (k+length-1<WIDTH)){
                                for(int k=0;k<length;k++){
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

        currentVoca = voca;
        int length = voca.voca.Length;

        int y;
        int x;

        // 
        while(complete==false)
        {
            y = Random.Range(0, HEIGHT);
            x = Random.Range(0, WIDTH);
            PlaceAlphabet(voca.voca, y, x, 0, length, 0);
        }
        // 
        for (int i = 0; i < WIDTH * HEIGHT; i++)
        {
            if (buttonWordPuzzles[i].alphabetejsw == ALPHABET.Empty)
                buttonWordPuzzles[i].alphabet = (ALPHABET)Random.Range(0, (int)ALPHABET.Length);
        }
        //
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