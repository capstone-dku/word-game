using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ALPHABET
{
    a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,Length, Empty
}

public class PanelWordPuzzle : MonoBehaviour
{
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField] private PanelItem panelItem;
    [SerializeField] private GameObject panelCorrect;
    [SerializeField] private GameObject panelWrong;
    [SerializeField] private SaveLoad saveLoad;

    public List<Sprite[]> sprites = new List<Sprite[]>();
    public Sprite[] spriteRed;
    public Sprite[] spriteBlue;
    public Sprite[] spritePurple;
    public Sprite[] spriteGreen;
    public Sprite[] spriteGrey;

    public ButtonWordPuzzle[] buttonWordPuzzles;
    private bool[] visited;
    private Tuple<int, int>[] direction = new Tuple<int, int>[]
    {
        // (Y, X)
        new Tuple<int, int>(1, 0), // UP
        new Tuple<int, int>(0, 1), // RIGHT
        new Tuple<int, int>(0, -1), // LEFT
        new Tuple<int, int>(-1, 0) // DOWN
    };

    private const int WIDTH = 4;
    private const int HEIGHT = 6;
    [SerializeField] private Text textRemainTime;
    [SerializeField] private Text textMeaingWord;

    private List<Voca> vocaList; // 퍼즐판에 출제될 단어 리스트
    private Voca currentVoca; // 현재 퍼즐판에 출제된 단어
    private StringBuilder sb; // 사용자가 클릭한 단어
    private int currentIndex; // 현재 퍼즐판에 출제된 단어의 인덱스
    private bool complete = false; // 버튼에 정답 단어가 완성되었는지
    private bool running = false; // 현재 퀴즈 시간이 흘러가고 있는지
    private int vocaSuccess; // 성공 횟수 저장
    private int[] vocaWeight; // 단어들 가중치 변화 후 값 저장
    private bool globalClick = false;
    private int lastIndex = -1;
    private Coroutine puzzleCoroutine = null;
    [Header("퀴즈 제한 시간")][Range(1,60)][SerializeField] private int time;
    private int currentTime;


    private void Start()
    {
        sprites.Add(spriteRed);
        sprites.Add(spriteBlue);
        sprites.Add(spritePurple);
        sprites.Add(spriteGreen);
        sprites.Add(spriteGrey);

        List<Voca> vocaList = vocaSelector.FindVocaWeight(5);

        vocaSuccess = 0;
        vocaWeight = new int[]{0,0,0,0,0};

        visited = new bool[buttonWordPuzzles.Length];
        sb = new StringBuilder();

        Init(vocaList);
    }

    void Update()
    {
        if(Input.GetMouseButton(0)){//누르고 있을 때 >> 단어 체크
            //globalClick=true;
        }
        else if(Input.GetMouseButtonUp(0)){//방금 땟을 때
            if(sb.Length>0){
                string s = sb.ToString();
                if (s.Equals(currentVoca.voca.ToLower()))
                {
                    OnPuzzleFinished(true);
                    globalClick=false;
                }
                else
                {
                    OnPuzzleFinished(false);
                }
            }
        }
        else{//땟을 경우
            if(globalClick==true){
                globalClick=false;
                sb.Clear();
                sb.Length = 0;
                for (int i = 0; i < HEIGHT * WIDTH; i++)
                {
                    buttonWordPuzzles[i].clicked = false;
                    buttonWordPuzzles[i].SetColor(Color.white);
                }
            }
        }
    }

    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        Clear();

        // TEST
        StartGame();
    }

    public void Clear()
    {
        for (int i = 0; i < HEIGHT * WIDTH; i++)
        {
            buttonWordPuzzles[i].Init(i, this);
            visited[i] = false;
        }

        sb.Clear();
        currentTime = time;

        complete = false;
        running = false;
        panelCorrect.SetActive(false);
        panelWrong.SetActive(false);
        UpdateSprites();
    }

    public void StartGame()
    {
        StartCoroutine(StartPuzzle(0));
    }
    public void UpdateSprites()
    {
        int color = Random.Range(0, sprites.Count);
        Sprite[] sprite = sprites[color];
        for (int i = 0; i < HEIGHT * WIDTH; i++)
        {
            buttonWordPuzzles[i].UpdateSprite(sprite[(int)buttonWordPuzzles[i].alphabet]);
        }
    }

    private void MakePuzzle(Voca voca)
    {
        currentVoca = voca;
        int length = voca.voca.Length;
        Debug.Log(voca.voca.ToString() + "퍼즐 만들기");

        int y;
        int x;

        // 정답 단어를 배치한다.
        while(complete==false)
        {
            y = Random.Range(0, HEIGHT);
            x = Random.Range(0, WIDTH);
            PlaceAlphabet(voca.voca, y, x, 0, length, 0);
        }
        // 정답 단어 이외의 칸을 임의의 알파벳으로 만든다.
        for (int i = 0; i < WIDTH * HEIGHT; i++)
        {
            if (buttonWordPuzzles[i].alphabet == ALPHABET.Empty)
                buttonWordPuzzles[i].alphabet = (ALPHABET)Random.Range(0, (int)ALPHABET.Length);
        }
        // 만든 알파벳으로 버튼 스프라이트를 갱신한다.
        UpdateSprites();

        textMeaingWord.text = voca.meaning[0];

    }

    private void PlaceAlphabet(string voca, int y, int x, int depth, int length, int stack)
    {
        if(stack>1000){
            return;
        }

        if (depth >= length || complete)
        {
            complete = true;
            return;
        }
        
        ALPHABET alphabet = (ALPHABET)Enum.Parse(typeof(ALPHABET), char.ToLower(voca[depth]).ToString());
        buttonWordPuzzles[y * WIDTH + x].alphabet = alphabet;
        // 랜덤으로 다음 노드 선정
        List<int> random = new List<int>() { 0, 1, 2, 3 };

        int ny = y, nx = x;
        bool found = false;
        while (random.Count > 0)
        {
            int rnd = Random.Range(0, random.Count);
            Tuple<int, int> next = direction[rnd];
            random.RemoveAt(rnd);
            ny = y + next.Item1;
            nx = x + next.Item2;
            // 무작위로 선정된 다음 노드를 탐색한다.
            if (CanPlaceAlphabet(ny, nx) && visited[ny*WIDTH+nx] == false && complete == false)
            {
                found = true;
                PlaceAlphabet(voca, ny, nx, depth + 1, length, stack+1);
            }
        }

        if (found == false)
        {
            // 현재 노드에서 더 이상 탐색할 노드가 없을때
            // 이전의 노드로 돌아간다.
            buttonWordPuzzles[y * WIDTH + x].alphabet = ALPHABET.Empty;
            visited[y * WIDTH + x] = true;
            if(CanPlaceAlphabet(y,x))
                PlaceAlphabet(voca, y, x, depth, length, stack+1);
        }
    }

    private bool CanPlaceAlphabet(int y, int x)
    {
        if (y < 0 || y >= HEIGHT) return false;
        if (x < 0 || x >= WIDTH) return false;
        if (buttonWordPuzzles[y*WIDTH + x].alphabet != ALPHABET.Empty) return false;
        return true;
    }

    IEnumerator StartPuzzle(int index, bool wait=false, float waitTime=1.0f)
    {
        if(wait)
            yield return new WaitForSeconds(waitTime);
        puzzleCoroutine = StartCoroutine(Puzzle(index));
    }

    IEnumerator Puzzle(int index)
    {
        Clear();
        running = true;
        currentIndex = index;
        MakePuzzle(vocaList[index]);
        while (currentTime > 0)
        {
            currentTime--;
            textRemainTime.text = (currentTime / 60).ToString() + " : " + (currentTime % 60).ToString();
            yield return new WaitForSeconds(1.0f);
        }

        if (currentTime <= 0 && running)
        {
            OnPuzzleFinished(false);
        }
        yield return null;
    }

    public void OnButtonClickAlphabet(ButtonWordPuzzle button)
    {
        globalClick = true;

        // 클릭
        button.SetColor(Color.gray);
        button.clicked = true;
        sb.Append(Enum.GetName(typeof(ALPHABET), button.alphabet));
        lastIndex = button.index;

        Debug.Log(sb.ToString());
    }

    public void OnButtonDragAlphabet(ButtonWordPuzzle button){
        if((globalClick == true) && (button.clicked == false))
        {
            int __x = lastIndex%4;
            int __y = lastIndex/4;
            int __bx = button.index%4;
            int __by = button.index/4;
            bool validCheck = false;

            if((Math.Abs(__x-__bx)==1)&&(__y==__by)){
                validCheck = true;
            }
            else if((Math.Abs(__y-__by)==1)&&(__x==__bx)){
                validCheck = true;
            }
            
            if(validCheck)
            {
                // 클릭
                button.SetColor(Color.gray);
                button.clicked = true;
                sb.Append(Enum.GetName(typeof(ALPHABET), button.alphabet));
                lastIndex = button.index;

                Debug.Log(sb.ToString());
            }
        }
    }

    public void OnPuzzleFinished(bool correct)
    {
        int weight= vocaSelector.getVocaWeight(vocaList[currentIndex]);
        if (correct)
        {
            // 맞았을때
            weight = (int)(weight/2);
            panelCorrect.SetActive(true);
            vocaSuccess+=1;
        }
        else
        {
            // 틀렸을때
            weight = weight*2;
            panelWrong.SetActive(true);

        }
        vocaWeight[currentIndex] = weight;      

        // 버튼 색 및 플래그 초기화
        for (int i = 0; i < buttonWordPuzzles.Length; i++)
        {
            buttonWordPuzzles[i].SetColor(Color.white);
            buttonWordPuzzles[i].clicked = false;
        }
        // 스트링 초기화
        sb.Clear();
        // 코루틴 정지
        running = false;
        StopCoroutine(puzzleCoroutine);
        // 다음 보카로 넘어감
        currentIndex++;
        if (currentIndex >= vocaList.Count)
        {
            // 퀴즈 완전히 끝났을때
            
            // 보상 지급
            saveLoad.AdjustCoin(0,50+(vocaSuccess*50));
            saveLoad.AdjustCoin(1,25+(vocaSuccess*25));
            panelItem.UpdateCoin(0, saveLoad.GetCoin(0));
            panelItem.UpdateCoin(1, saveLoad.GetCoin(1));
            // 정답, 오답 단어 가중치 변경
            vocaSelector.SaveVocaWeight(vocaList, vocaWeight);
            // 데이터 저장
            saveLoad.SaveData();
            return;
        }
        puzzleCoroutine = StartCoroutine(StartPuzzle(currentIndex, true));
    }

    public string aaa(){
        return "aaa";
    }
}
