using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CWList
{
    Voca voca;
    bool Row; // 0
    
}

public class PanelCrossWord : MonoBehaviour
{
    [SerializeField] private GameObject panelKeyboard;
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField] private SaveLoad saveLoad;
    private const int VOCA_NUM = 5; // 퍼즐판 안에 몇개의 단어가 들어갈지
    private const int WIDTH = 15; // 퍼즐판의 크기
    private const int HEIGHT = 15; // 퍼즐판의 크기

    private List<Voca> vocaList; // 퍼즐판에 출제될 단어 리스트

    public char[][] wordPuzzle; // 퍼즐판. 0의 경우 비어 있음 이외의 경우 채워야하는 답으로 문자형태로 들어가있음.
    public int complete = 0; // 몇개의 단어가 퍼즐판에 들어갔는지.

    private void Start()
    {
        List<Voca> vl = vocaSelector.FindVocaWeight(10);
        MakePuzzle(vl);
    }

    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        //Clear();
    }

    public void Clear()
    {
        for(int i=0;i<WIDTH;i++){
            for(int j=0;j>HEIGHT;j++){
                wordPuzzle[i][j]='0';
            }
        }
        complete = 0;
    }

    private void MakePuzzle(List<Voca> vocaList)
    {
        int x, y, len;
        for(int i=0 ; (i<vocaList.Count) && (complete < VOCA_NUM) ; i++)//긴 단어 순으로 word 정렬
        {
            int temp = i;
            int tempLength = vocaList[i].voca.Length;
            for(int j=i+1 ; j<VOCA_NUM ; j++)
            {
                if(vocaList[j].voca.Length > tempLength )
                {
                    temp = j;
                    tempLength = vocaList[j].voca.Length;
                }
                Voca tempVoca = vocaList[i];
                vocaList[i] = vocaList[j];
                vocaList[j] = tempVoca;
            }
        }

        for(int i=0 ; (i<vocaList.Count) && (complete < VOCA_NUM) ; i++)
        {
            len = vocaList[i].voca.Length;
            if(i==0)//첫 단어
            {
                if(Random.Range(0,2)<1){//가로 
                    x = Random.Range(0, WIDTH-len);
                    y = Random.Range(0, HEIGHT); 

                    y = Random.Range(0, HEIGHT-len);
                    x = Random.Range(0, WIDTH-len);
                    for(int j=0;j<len;j++){
                        wordPuzzle[x+j][y] = vocaList[i].voca[j];
                    }
                }
                else{//세로
                    x = Random.Range(0, WIDTH);
                    y = Random.Range(0, HEIGHT-len);

                    y = Random.Range(0, HEIGHT-len);
                    x = Random.Range(0, WIDTH-len);
                    for(int j=0;j<len;j++){
                        wordPuzzle[x][y+j] = vocaList[i].voca[j];
                    }
                }
                complete+=1;
            }
            else{//두번 째 부터
                len = vocaList[i].voca.Length;
                bool suc = false;
                for(int count = 0; (count < 100) && (suc == false); count++){//겹치는 부분 확인 count 횟수 만큼 검색 << 나중에 리팩토링
                    x = Random.Range(0, WIDTH);
                    y = Random.Range(0, HEIGHT);

                    for(int j=0; j<len;j++){
                        if(wordPuzzle[x][y] == vocaList[i].voca[j]){
                            bool checkValid = false;
                            int checkRow = 0;
                            int checkCol = 0;
                            if(x-1 >= 0){
                                if(wordPuzzle[x-1][y] != '0'){
                                    checkRow = 1;
                                }
                            }
                            if(x+1 < WIDTH){
                                if(wordPuzzle[x+1][y] != '0'){
                                    checkRow = 1;
                                }
                            }
                            if(y-1 >= 0){
                                if(wordPuzzle[x][y-1] != '0'){
                                    checkCol = 1;
                                }
                            }
                            if(y+1 < HEIGHT){
                                if(wordPuzzle[x][y+1] != '0'){
                                    checkCol = 1;
                                }
                            }
                            if(checkRow == 0){//가로가 비어있을 경우 (x-j가 시작점)
                                checkValid = true;
                                if(x-j-1>=0){
                                    if(wordPuzzle[x-j-1][y] != '0'){
                                        checkValid = false;
                                    }
                                }
                                if(x-j+len < WIDTH){
                                    if(wordPuzzle[x-j+len][y] != '0'){
                                        checkValid = false;
                                    }
                                }
                                if(checkValid && (x-j>0) && (x+len-j<=WIDTH)){
                                    for(int k=0;k<len;k++){
                                        if(wordPuzzle[x-j+k][y]!= '0'){
                                            checkValid = false;
                                        }
                                    }
                                }
                            }
                            else if(checkCol == 0){//세로가 비어있을 경우 (y-j가 시작점)
                                checkValid = true;
                                if(y-j-1>=0){
                                    if(wordPuzzle[x][y-j-1] != '0'){
                                        checkValid = false;
                                    }
                                }
                                if(y-j+len < HEIGHT){
                                    if(wordPuzzle[x][y-j+len] != '0'){
                                        checkValid = false;
                                    }
                                }
                                if(checkValid && (y-j>0) && (y+len-j<=HEIGHT)){
                                    for(int k=0;k<len;k++){
                                        if(wordPuzzle[x][y-j+k]!= '0'){
                                            checkValid = false;
                                        }
                                    }
                                }
                            }
                            if(checkValid){//조건을 달성한 경우
                                if(checkRow == 0){
                                    for(int k=0;k<len;k++){
                                        wordPuzzle[x-j+k][y]=vocaList[i].voca[j];
                                    }
                                    suc = true;
                                    complete+=1;
                                }
                                else if(checkCol == 0){
                                    for(int k=0;k<len;k++){
                                        wordPuzzle[x][y-j+k]=vocaList[i].voca[j];
                                    }
                                    suc = true;
                                    complete+=1;
                                }
                                else{
                                    Debug.Log("올바르지 않은 생성");   
                                }
                            }
                        }
                    }
                }
            }
        }

        Debug.Log(complete);
        if(complete == VOCA_NUM){
            for(int i = 0; i<WIDTH ;i++){
                Debug.Log(new string(wordPuzzle[i]));
            }
        }

        /*
        while(complete)
        {
            y = Random.Range(0, HEIGHT);
            x = Random.Range(0, WIDTH);
            PlaceAlphabet(voca.voca, y, x, 0, length, 0);
        }
         
        for (int i = 0; i < WIDTH * HEIGHT; i++)
        {
            if (buttonWordPuzzles[i].alphabetejsw == ALPHABET.Empty)
                buttonWordPuzzles[i].alphabet = (ALPHABET)Random.Range(0, (int)ALPHABET.Length);
        }
        */
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