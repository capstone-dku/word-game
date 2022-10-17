using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CWList
{
    public int x; // 시작지점
    public int y; // 시작지점
    public int angle; // 각도 0 : 가로, 1 : 세로
    public String voca; // 답
    
    public CWList(int x, int y, int angle, String voca)
    {
        this.x = x;
        this.y = y;
        this.angle = angle;
        this.voca = voca;
    }
}

public class PanelCrossWord : MonoBehaviour
{
    [SerializeField] private GameObject panelKeyboard;
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField] private SaveLoad saveLoad;
    private const int VOCA_NUM = 10; // 퍼즐판 안에 몇개의 단어가 들어갈지 
    private const int WIDTH = 15; // 퍼즐판의 크기
    private const int HEIGHT = 15; // 퍼즐판의 크기
    private const int VOCA_NUM2 = 20; // 유효체크할 단어의 개수

    private List<Voca> vocaList; // 퍼즐판에 출제될 단어 리스트

    public char[,] wordPuzzle; // 퍼즐판. 0의 경우 비어 있음 이외의 경우 채워야하는 답으로 문자형태로 들어가있음.
    public List<int> alPosition; // 퍼즐판에 배치된 알파벳의 위치를 담은 배열.
    public List<CWList> cwList;
    public int complete = 0; // 몇개의 단어가 퍼즐판에 들어갔는지.

    private void Start()
    {
        char aa = 'a';
        aa = (char)((int)aa-32); 
        Debug.Log("A나와야함 : "+aa);
        List<Voca> vl = vocaSelector.FindVocaWeight(VOCA_NUM2);
        Init(vl);
        MakePuzzle(vl);
    }

    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        Clear();
    }

    public void Clear()
    {
        wordPuzzle = new char[WIDTH,HEIGHT];
        alPosition = new List<int>();
        cwList = new List<CWList>();
        for(int i=0;i<WIDTH;i++){
            for(int j=0;j<HEIGHT;j++){
                wordPuzzle[i,j]='0';
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
            Debug.Log(vocaList[i].voca);
            len = vocaList[i].voca.Length;
            if(i==0)//첫 단어
            {
                if(Random.Range(0,2)<1)//가로 
                {
                    x = (int)((WIDTH-len)/4)+Random.Range(0, (int)((WIDTH-len)/2));
                    y = (int)(HEIGHT/4)+Random.Range(0, (int)(HEIGHT/2)); 
                    for(int j=0;j<len;j++)
                    {
                        Debug.Log("체크2:"+vocaList.Count.ToString());
                        wordPuzzle[x+j,y] = vocaList[i].voca[j];
                        alPosition.Add((x+j)*HEIGHT+ y);
                    }
                    cwList.Add(new CWList(x,y,0,vocaList[i].voca));
                }
                else//세로
                {
                    x = (int)(WIDTH/4)+Random.Range(0, (int)(WIDTH/2)); 
                    y = Random.Range(0, HEIGHT-len);
                    for(int j=0;j<len;j++)
                    {
                        Debug.Log("체크2:"+vocaList.Count.ToString());
                        wordPuzzle[x,y+j] = vocaList[i].voca[j];
                        alPosition.Add((x)*HEIGHT+y+j);
                    }
                    cwList.Add(new CWList(x,y,1,vocaList[i].voca));
                }
                complete+=1;
            }
            else
            {//두번 째 부터
                len = vocaList[i].voca.Length;
                bool suc = false;
                for(int count = 0; (count < 10) && (suc == false); count++)//겹치는 부분 확인 count 횟수 만큼 검색 << 나중에 리팩토링 << 똑같은 단어 2번 나올떄 있는거같음
                {
                    int position = (int)Random.Range(0, alPosition.Count);
                    x = (int)(alPosition[position]/HEIGHT);
                    y = (int)(alPosition[position]%HEIGHT);

                    for(int j=0; j<len;j++)
                    {
                        if(wordPuzzle[x,y] == vocaList[i].voca[j])
                        {
                            bool checkValid = false;
                            int checkRow = 0;
                            int checkCol = 0;
                            if(x-1 >= 0)
                            {
                                if(wordPuzzle[x-1,y] != '0')
                                {
                                    checkRow = 1;
                                }
                            }
                            if(x+1 < WIDTH)
                            {
                                if(wordPuzzle[x+1,y] != '0')
                                {
                                    checkRow = 1;
                                }
                            }
                            if(y-1 >= 0)
                            {
                                if(wordPuzzle[x,y-1] != '0')
                                {
                                    checkCol = 1;
                                }
                            }
                            if(y+1 < HEIGHT)
                            {
                                if(wordPuzzle[x,y+1] != '0')
                                {
                                    checkCol = 1;
                                }
                            }
                            if(checkRow == 0){//가로가 비어있을 경우 (x-j가 시작점)
                                checkValid = true;
                                if(x-j<0)
                                {
                                    checkValid = false;
                                }
                                else if(x-j-1>=0)
                                {
                                    if(wordPuzzle[x-j-1,y] != '0')
                                    {
                                        checkValid = false;
                                    }
                                }
                                if(x-j+len > WIDTH)
                                {
                                    checkValid = false;
                                }
                                else if(x-j+len < WIDTH)
                                {
                                    if(wordPuzzle[x-j+len,y] != '0')
                                    {
                                        checkValid = false;
                                    }
                                }
                                if(checkValid && (x-j>0) && (x+len-j<=WIDTH))
                                {
                                    for(int k=0;k<len;k++)
                                    {
                                        if(wordPuzzle[x-j+k,y]!= '0')
                                        {
                                            if(wordPuzzle[x-j+k,y]!=vocaList[i].voca[k])
                                            {
                                                checkValid = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else if(checkCol == 0)//세로가 비어있을 경우 (y-j가 시작점)
                            {
                                checkValid = true;
                                if(y-j<0)
                                {
                                    checkValid = false;
                                }
                                else if(y-j-1>=0)
                                {
                                    if(wordPuzzle[x,y-j-1] != '0')
                                    {
                                        checkValid = false;
                                    }
                                }
                                if(y-j+len > WIDTH)
                                {
                                    checkValid = false;
                                }
                                else if(y-j+len < HEIGHT)
                                {
                                    if(wordPuzzle[x,y-j+len] != '0')
                                    {
                                        checkValid = false;
                                    }
                                }
                                if(checkValid && (y-j>0) && (y+len-j<=HEIGHT))
                                {
                                    
                                    for(int k=0;k<len;k++)
                                    {
                                        if(wordPuzzle[x,y-j+k]!= '0')
                                        {
                                            if(wordPuzzle[x,y-j+k]!=vocaList[i].voca[k]){

                                                checkValid = false;
                                            }
                                        }
                                    }
                                }
                            }
                            if(checkValid)
                            {//조건을 달성한 경우
                                if(checkRow == 0)
                                {
                                    for(int k=0;k<len;k++)
                                    {
                                        wordPuzzle[x-j+k,y]=vocaList[i].voca[k];
                                        alPosition.Add((x-j+k)*HEIGHT+ y);
                                    }
                                    suc = true;
                                    complete+=1;
                                    cwList.Add(new CWList(x,y,0,vocaList[i].voca));
                                    Debug.Log("생성성공");  
                                }
                                else if(checkCol == 0)
                                {
                                    for(int k=0;k<len;k++)
                                    {
                                        wordPuzzle[x,y-j+k]=vocaList[i].voca[k];
                                        alPosition.Add((x)*HEIGHT+ y-j+k);
                                    }
                                    suc = true;
                                    complete+=1;
                                    cwList.Add(new CWList(x,y,1,vocaList[i].voca));
                                    Debug.Log("생성성공");  
                                }
                                else
                                {
                                    Debug.Log("올바르지 않은 생성");   
                                }
                            }
                        }
                    }
                }
            }
        }

        Debug.Log("배치된 단어의 개수 : "+complete.ToString());
        string str = "";
        for(int i = 0; i < WIDTH ;i++)
        {
            for(int j = 0; j < HEIGHT; j++)
            {
                if(wordPuzzle[i,j] != '0')
                {
                    str = str + wordPuzzle[i,j] +"\t";
                }
                else{
                    str = str + "\t";
                }
            }
            str=str+"\r\n";
        }
        Debug.Log(str);

        for(int i=0;i<WIDTH;i++)
        {
            for(int j=0;j<HEIGHT;j++)
            {
                if(wordPuzzle[i,j]!='0')
                {
                    wordPuzzle[i,j]='1';
                }
            }
        }

        str = "";
        for(int i = 0; i < WIDTH ;i++)
        {
            for(int j = 0; j < HEIGHT; j++)
            {
                if(wordPuzzle[i,j] != '0')
                {
                    str = str + wordPuzzle[i,j] +"\t";
                }
                else{
                    str = str + "0\t";
                }
            }
            str=str+"\r\n";
        }
        Debug.Log(str);
    }

    public void CheckAnswer(int x, int y)
    {
        for(int i=0; i<cwList.Count; i++)
        {
            if(cwList[i].x == x){//x 좌표에 일치하는 단어가 있을경우
                bool success = true;
                if(cwList[i].angle == 1)//x가 같으므로 세로 확인
                {
                    for(int j=0; j<cwList[i].voca.Length;j++)
                    {
                        if((cwList[i].voca[j] != wordPuzzle[cwList[i].x,cwList[i].y+j]) && (cwList[i].voca[j] != wordPuzzle[cwList[i].x,cwList[i].y+j]+32)) // 대문자와 소문자 모두 체크
                        {
                            success = false;
                        }
                    }
                    if(success){
                        for(int j=0; j<cwList[i].voca.Length;j++)
                        {
                            if(wordPuzzle[cwList[i].x,cwList[i].y+j]>95)//소문자 일 경우
                            {
                                wordPuzzle[cwList[i].x,cwList[i].y+j]=(char)((int)(wordPuzzle[cwList[i].x,cwList[i].y+j])-32); 
                                /*
                                    cwList[i].x , cwList[i].y+j 에 있는 패널이 수정 불가능 하다는 것을 시각적으로 표시해야함.
                                */
                            }
                        }
                    }
                }
            }
            if(cwList[i].y == y)//y 좌표에 일치하는 단어가 있을 경우
            {
                bool success = true;
                if(cwList[i].angle == 0)//y가 같으므로 가로 확인
                {
                    for(int j=0; j<cwList[i].voca.Length;j++)
                    {
                        if((cwList[i].voca[j] != wordPuzzle[cwList[i].x+j,cwList[i].y]) && (cwList[i].voca[j] != wordPuzzle[cwList[i].x+j,cwList[i].y]+32)) // 대문자와 소문자 모두 체크
                        {
                            success = false;
                        }
                    }
                    if(success){
                        for(int j=0; j<cwList[i].voca.Length;j++)
                        {
                            if(wordPuzzle[cwList[i].x+j,cwList[i].y]>95)//소문자 일 경우
                            {
                                wordPuzzle[cwList[i].x+j,cwList[i].y]=(char)((int)(wordPuzzle[cwList[i].x+j,cwList[i].y])-32); 
                                /*
                                    cwList[i].x+j, cwList[i].y 에 있는 패널이 수정 불가능 하다는 것을 시각적으로 표시해야함.
                                */
                            }
                        }
                    }
                }
            }
        }
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