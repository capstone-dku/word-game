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

public class CWList
{
    public int x; // 시작지점
    public int y; // 시작지점
    public int angle; // 각도 0 : 가로, 1 : 세로
    public string voca; // 답
    public string meaning; // 뜻
    
    public CWList(int x, int y, int angle, string voca, string meaning)
    {
        this.x = x;
        this.y = y;
        this.angle = angle;
        this.voca = voca;
        this.meaning = meaning;
    }
}

public class PanelCrossWord : MonoBehaviour
{
    public List<Sprite[]> sprites = new List<Sprite[]>();
    public Sprite[] spriteRed;
    public Sprite[] spriteBlue;
    public Sprite[] spritePurple;
    public Sprite[] spriteGreen;
    public Sprite[] spriteGrey;

    public List<Sprite[]> answerSprites = new List<Sprite[]>();
    public Sprite[] answerSpriteRed;
    public Sprite[] answerSpriteBlue;
    public Sprite[] answerSpritePurple;
    public Sprite[] answerSpriteGreen;
    public Sprite[] answerSpriteGrey;

    [SerializeField] private GameObject panelCorrect;
    [SerializeField] private GameObject panelWrong;

    [SerializeField] private GameObject panelKeyboard;
    [SerializeField] private GameObject panelBoard;
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField] private SaveLoad saveLoad;
    private const int VOCA_NUM = 10; // 퍼즐판 안에 몇개의 단어가 들어갈지 
    private const int WIDTH = 15; // 퍼즐판의 크기
    private const int HEIGHT = 15; // 퍼즐판의 크기
    private const int VOCA_NUM2 = 20; // 유효체크할 단어의 개수

    [SerializeField] public ButtonCrossWord[] buttonCrossWords;
    [SerializeField] public Button[] buttonWord; // 밑에 나오는 단어 뜻 버튼

    private List<Voca> vocaList; // 퍼즐판에 출제될 단어 리스트

    private char[,] wordPuzzle; // 퍼즐판. 0의 경우 비어 있음 이외의 경우 채워야하는 답으로 문자형태로 들어가있음.
    private char[,] userInput;
    private List<int> alPosition; // 퍼즐판에 배치된 알파벳의 위치를 담은 배열.
    private List<CWList> cwList;
    private int complete = 0; // 몇개의 단어가 퍼즐판에 들어갔는지.

    private bool keyboardInput = false;
    private int inputX = 0;
    private int inputY = 0;
    private int color = 0;
    private List<ButtonCrossWord> currentInputButtons;
    private int currentInputIndex = 0;
    private int currentCWIndex = 0;

    private int currentTime = 0;
    [SerializeField] private Text textRemainTime;
    private int answerCount = 0;

    private Coroutine crossWordCoroutine = null;
    
    private void Awake()
    {
        var buttonKeyboard = panelKeyboard.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttonKeyboard.Length; i++)
        {
            int idx = i;
            buttonKeyboard[i].onClick.AddListener(() => OnClickedKeyboard(buttonKeyboard[idx].gameObject));
        }

        for (int i = 0; i < buttonWord.Length; i++)
        {
            int idx = i;
            buttonWord[i].onClick.AddListener(() => BeginKeyboardInput(idx));
        }
    }

    public void Init(List<Voca> vocaList)
    {
        sprites.Add(spriteRed);
        sprites.Add(spriteBlue);
        sprites.Add(spritePurple);
        sprites.Add(spriteGreen);
        sprites.Add(spriteGrey);

        answerSprites.Add(answerSpriteRed);
        answerSprites.Add(answerSpriteBlue);
        answerSprites.Add(answerSpritePurple);
        answerSprites.Add(answerSpriteGreen);
        answerSprites.Add(answerSpriteGrey);

        this.vocaList = vocaList;
        currentInputButtons = new List<ButtonCrossWord>();
        Clear();
    }

    public void Clear()
    {
        wordPuzzle = new char[WIDTH,HEIGHT];
        userInput = new char[WIDTH, HEIGHT];
        alPosition = new List<int>();
        cwList = new List<CWList>();

        currentInputButtons.Clear();
        currentInputIndex = 0; 
        currentCWIndex = 0;
        answerCount = 0;
        currentTime = 180;

        for (int i=0;i<WIDTH;i++)
        {
            for(int j=0;j<HEIGHT;j++)
            {
                wordPuzzle[i, j] = '0';
                userInput[i, j] = '0';
            }
        }
        complete = 0;
    }

    public void StartGame()
    {
        List<Voca> vl = vocaSelector.FindVocaWeight(VOCA_NUM2);
        crossWordCoroutine = StartCoroutine(GameStart(vl));
    }

    IEnumerator GameStart(List<Voca> vl)
    {
        while (complete < VOCA_NUM)
        {
            Init(vl);
            Clear();
            MakePuzzle(vl);
        }
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
            gameObject.SetActive(false);

        }
        yield return null;
    }

    private IEnumerator GameFinished()
    {
        panelCorrect.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }
    private void MakePuzzle(List<Voca> vocaList)
    {
        for(int i=0; i<vocaList.Count ; i++){
            Debug.Log(vocaList[i].voca);
        }
        Debug.Log("섞기 전후");
        int x, y, len;
        for(int i=0 ; i<vocaList.Count ; i++)//긴 단어 순으로 word 정렬
        {
            int temp = i;
            int j = i+1;
            int tempLength = vocaList[i].voca.Length;
            for( ; j<vocaList.Count ; j++)
            {
                if(vocaList[j].voca.Length > tempLength )
                {
                    temp = j;
                    tempLength = vocaList[j].voca.Length;
                }
            }
            Voca tempVoca = vocaList[i];
            vocaList[i] = vocaList[temp];
            vocaList[temp] = tempVoca;
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
                        wordPuzzle[x+j,y] = vocaList[i].voca[j];
                        alPosition.Add((x+j)*HEIGHT+ y);
                    }
                    cwList.Add(new CWList(x, y, 0, vocaList[i].voca, vocaList[i].meaning[0]));
                }
                else//세로
                {
                    x = (int)(WIDTH/4)+Random.Range(0, (int)(WIDTH/2)); 
                    y = Random.Range(0, HEIGHT-len);
                    for(int j=0;j<len;j++)
                    {
                        wordPuzzle[x,y+j] = vocaList[i].voca[j];
                        alPosition.Add((x)*HEIGHT+y+j);
                    }
                    cwList.Add(new CWList(x,y,1,vocaList[i].voca, vocaList[i].meaning[0]));
                }
                complete+=1;
            }
            else
            {//두번 째 부터
                len = vocaList[i].voca.Length;
                bool suc = false;
                for(int count = 0; (count < 10 + (complete*5)) && (suc == false); count++)//겹치는 부분 확인 count 횟수 만큼 검색 << 나중에 리팩토링 << 똑같은 단어 2번 나올떄 있는거같음
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
                                    if(y-1>=0){
                                        for(int k=0;k<len;k++)
                                        {
                                            if(wordPuzzle[x-j+k,y-1]!= '0')
                                            {
                                                if(wordPuzzle[x-j+k,y]!=vocaList[i].voca[k])
                                                {
                                                    checkValid = false;
                                                }
                                            }
                                        }
                                    }
                                    if(y+1<HEIGHT){
                                        for(int k=0;k<len;k++)
                                        {
                                            if(wordPuzzle[x-j+k,y+1]!= '0')
                                            {
                                                if(wordPuzzle[x-j+k,y]!=vocaList[i].voca[k])
                                                {
                                                    checkValid = false;
                                                }
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
                                    if(x-1>=0){
                                        for(int k=0;k<len;k++)
                                        {
                                            if(wordPuzzle[x-1,y-j+k]!= '0')
                                            {
                                                if(wordPuzzle[x,y-j+k]!=vocaList[i].voca[k]){

                                                    checkValid = false;
                                                }
                                            }
                                        }
                                    }
                                    if(x+1<WIDTH){
                                        for(int k=0;k<len;k++)
                                        {
                                            if(wordPuzzle[x+1,y-j+k]!= '0')
                                            {
                                                if(wordPuzzle[x,y-j+k]!=vocaList[i].voca[k]){

                                                    checkValid = false;
                                                }
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
                                    cwList.Add(new CWList(x,y,0,vocaList[i].voca, vocaList[i].meaning[0]));
                                    //Debug.Log("생성성공");  
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
                                    cwList.Add(new CWList(x,y,1,vocaList[i].voca, vocaList[i].meaning[0]));
                                    //Debug.Log("생성성공");  
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
                    Debug.Log(wordPuzzle[i,j]);
                }
                else{
                    str = str + "\t";
                }
            }
            str=str+"\r\n";
        }
        Debug.Log(str);

        for (int i=0;i<WIDTH;i++)
        {
            for(int j=0;j<HEIGHT;j++)
            {
                if(wordPuzzle[i,j]!='0')
                {
                    // wordPuzzle[i,j]='1';
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

        // 워드 퍼즐판을 채운다.
        color = Random.Range(0, sprites.Count);
        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                if (wordPuzzle[i, j] != '0')
                {
                    int idx = wordPuzzle[i, j] - 'a';
                    // 정답 표시
                    // buttonCrossWords[i * WIDTH + j].GetComponent<Image>().sprite = sprites[color][idx];
                    buttonCrossWords[i * WIDTH + j].GetComponent<Image>().sprite = sprites[color][26];
                    buttonCrossWords[i * WIDTH + j].GetComponent<Button>().interactable = true;

                }
                else
                {
                    buttonCrossWords[i * WIDTH + j].GetComponent<Image>().sprite = sprites[color][27];
                    buttonCrossWords[i * WIDTH + j].GetComponent<Button>().interactable = false;
                }

                buttonCrossWords[i * WIDTH + j].x = i;
                buttonCrossWords[i * WIDTH + j].y = j;
                buttonCrossWords[i * WIDTH + j].correct = false;
            }
        }

        for (int i = 0; i < cwList.Count; i++)
        {
            buttonWord[i].GetComponentInChildren<Text>().text = cwList[i].meaning;
        }
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
                                buttonCrossWords[cwList[i].x*15 + cwList[i].y+j].GetComponent<Image>().color = Color.black;
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
    private void BeginKeyboardInput(int idx)
    {
        ShowKeyboard(true);
        currentInputButtons.Clear();
        currentInputIndex = 0;
        currentCWIndex = idx;
        int x = cwList[idx].x;
        int y = cwList[idx].y;

        if (cwList[idx].angle == 1)
        {
            for (int i = 0; i < WIDTH; i++)
            {
                if (wordPuzzle[x, i] == cwList[idx].voca[0])
                {
                    bool success = true;
                    for (int j = 0; j < cwList[idx].voca.Length; j++)
                    {
                        if (i + j >= HEIGHT)
                        {
                            success = false;
                            break;
                        }
                        if (wordPuzzle[x, i + j] != cwList[idx].voca[j])
                        {
                            i = i + j;
                            success = false;
                            break;
                        }
                    }

                    if (success)
                    {
                        for (int j = 0; j < cwList[idx].voca.Length; j++)
                        {
                            if (!buttonCrossWords[x*15+i+j].correct)
                                buttonCrossWords[x * 15 + i + j].GetComponent<Image>().color = Color.red;
                            // if (userInput[x, i+j] == '0')
                            currentInputButtons.Add(buttonCrossWords[x * 15 + i + j]);
                        }
                    }
                }
            }
        }

        if (cwList[idx].angle == 0)
        {
            for (int i = 0; i < HEIGHT; i++)
            {
                if (wordPuzzle[i, y] == cwList[idx].voca[0])
                {
                    bool success = true;
                    for (int j = 0; j < cwList[idx].voca.Length; j++)
                    {
                        if (i + j >= HEIGHT)
                        {
                            success = false;
                            break;
                        };
                        if (wordPuzzle[i+j,y] != cwList[idx].voca[j])
                        {
                            i = i + j;
                            success = false;
                            break;
                        }
                    }

                    if (success)
                    {
                        for (int j = 0; j < cwList[idx].voca.Length; j++)
                        {
                            if (!buttonCrossWords[(i + j) * 15 + y].correct)
                                buttonCrossWords[(i + j) * 15 + y].GetComponent<Image>().color = Color.red;
                            // if (userInput[(i+j), y] == '0')
                            currentInputButtons.Add(buttonCrossWords[(i+j)*15 + y]);
                        }
                    }
                }
            }
        }
    }
    private void ShowKeyboard(bool show)
    {
        panelKeyboard.SetActive(show);
        keyboardInput = show;
        for (int i = 0; i < buttonWord.Length; i++)
            buttonWord[i].gameObject.SetActive(!show);
    }

    public void OnClickedKeyboard(GameObject button)
    {
        if (keyboardInput)
        {
            string input = button.gameObject.name;
            if (input.Equals("Backspace"))
            {
                // 임시
                ShowKeyboard(false);
                for (int i = 0; i < currentInputButtons.Count; i++)
                {
                    currentInputButtons[i].GetComponent<Image>().color = Color.white;
                    if (!currentInputButtons[i].correct)
                        currentInputButtons[i].GetComponent<Image>().sprite = sprites[color][26];
                }
                currentInputButtons.Clear();
                currentInputIndex = 0;
            }
            else
            {
                Debug.Log(input);
                char alphabet = input[0];

                while (currentInputButtons[currentInputIndex].correct)
                {
                    currentInputIndex++;
                }
                
                if (currentInputIndex >= currentInputButtons.Count)
                {
                    currentInputIndex = 0;
                    CheckAnswer();
                    return;
                }
                currentInputButtons[currentInputIndex].GetComponent<Image>().color = Color.white;
                currentInputButtons[currentInputIndex].GetComponent<Image>().sprite = sprites[color][alphabet - 'A'];
                int x = currentInputButtons[currentInputIndex].x;
                int y = currentInputButtons[currentInputIndex].y;
                userInput[x,y] = Char.ToLower(alphabet);
                currentInputIndex++;
                if (currentInputIndex >= currentInputButtons.Count)
                {
                    currentInputIndex = 0;
                    CheckAnswer();
                    return;
                }
                if (currentInputButtons[currentInputIndex].correct && currentInputIndex == currentInputButtons.Count-1)
                {
                    currentInputIndex = 0;
                    CheckAnswer();
                    return;
                }
                // buttonCrossWords[inputX * 15 + inputY].GetComponent<Image>().sprite = sprites[color][alphabet - 'A'];
            }
        }
    }
    private void CheckAnswer()
    {
        bool success = true;
        for (int i = 0; i < currentInputButtons.Count; i++)
        {
            int x = currentInputButtons[i].x;
            int y = currentInputButtons[i].y;
            if (wordPuzzle[x, y] != userInput[x, y])
            {
                success = false;
                break;
            }
        }

        if (success)
        {
            Debug.Log("정답");
            for (int i = 0; i < currentInputButtons.Count; i++)
            {
                int x = currentInputButtons[i].x;
                int y = currentInputButtons[i].y;
                char alphabet = userInput[x, y];
                currentInputButtons[i].GetComponent<Image>().sprite = answerSprites[color][alphabet - 'a'];
                currentInputButtons[i].GetComponent<Image>().color = Color.white;
                currentInputButtons[i].correct = true;
            }

            answerCount++;
            buttonWord[currentCWIndex].interactable = false;
            if (answerCount >= VOCA_NUM)
            {
                StartCoroutine(GameFinished());
            }
        }
        else
        {
            Debug.Log("오답");
            for (int i = 0; i < currentInputButtons.Count; i++)
            {
                int x = currentInputButtons[i].x;
                int y = currentInputButtons[i].y;
                char alphabet = userInput[x, y];

                if (currentInputButtons[i].correct)
                {
                    currentInputButtons[i].GetComponent<Image>().color = Color.white;
                    continue;
                }

                currentInputButtons[i].GetComponent<Image>().sprite = sprites[color][26];
                currentInputButtons[i].GetComponent<Image>().color = Color.red;
                currentInputButtons[i].correct = false;
            }
            currentInputIndex = 0;
        }
    }
}