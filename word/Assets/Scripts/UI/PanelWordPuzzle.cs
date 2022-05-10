using System;
using System.Collections;
using System.Collections.Generic;
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

    public List<Sprite[]> sprites = new List<Sprite[]>();
    public Sprite[] spriteRed;
    public Sprite[] spriteBlue;
    public Sprite[] spritePurple;
    public Sprite[] spriteGreen;
    public Sprite[] spriteGrey;

    public ButtonWordPuzzle[] buttonWordPuzzles;
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
    private List<Voca> vocaList;
    private Voca currentVoca;
    private string currentString;
    private bool complete = false;
    private int time;

    private void Start()
    {
        sprites.Add(spriteRed);
        sprites.Add(spriteBlue);
        sprites.Add(spritePurple);
        sprites.Add(spriteGreen);
        sprites.Add(spriteGrey);

        List<Voca> vocaList = vocaSelector.FindVocaWeight(5);
        Init(vocaList);
    }

    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        time = 60;
        for (int i = 0; i < HEIGHT * WIDTH; i++)
        {
            buttonWordPuzzles[i].Init(i);
        }
        

        UpdateSprites();
        MakePuzzle(vocaList[0]);
        StartPuzzle();
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

        int y = Random.Range(0, HEIGHT);
        int x = Random.Range(0, WIDTH);

        // 정답 단어를 배치한다.
        PlaceAlphabet(voca.voca, y, x, 0, length);
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

    private void PlaceAlphabet(string voca, int y, int x, int depth, int length)
    {
        if (depth >= length || complete)
        {
            complete = true;
            return;
        }
        ALPHABET alphabet = (ALPHABET)Enum.Parse(typeof(ALPHABET), voca[depth].ToString());
        buttonWordPuzzles[y * WIDTH + x].alphabet = alphabet;
        // 랜덤으로 다음 위치 선정
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
            if (CanPlaceAlphabet(ny, nx) && complete == false)
            {
                found = true;
                PlaceAlphabet(voca, ny, nx, depth + 1, length);
            }
        }

        if (found == false)
        {
            buttonWordPuzzles[y * WIDTH + x].alphabet = ALPHABET.Empty;
            PlaceAlphabet(voca, y, x, depth, length);
        }
    }



private bool CanPlaceAlphabet(int y, int x)
    {
        if (y < 0 || y >= HEIGHT) return false;
        if (x < 0 || x >= WIDTH) return false;
        if (buttonWordPuzzles[y*WIDTH + x].alphabet != ALPHABET.Empty) return false;
        return true;
    }

    public void StartPuzzle()
    {

        StartCoroutine(Puzzle());
    }

    IEnumerator Puzzle()
    {
        while (time >= 0)
        {
            time--;
            textRemainTime.text = (time / 60).ToString() + " : " + (time % 60).ToString();
            yield return new WaitForSeconds(1.0f);
        }
        yield return null;
    }

    public void OnButtonClickAlphabet(ButtonWordPuzzle button)
    {
        if (button.clicked)
        {
            // 클릭 취소
            button.SetColor(Color.white);
            button.clicked = false;
            currentString.Remove(currentString.Length - 1);
        }
        else
        {
            // 클릭
            button.SetColor(Color.gray);
            button.clicked = true;
            currentString += Enum.GetName(typeof(ALPHABET), button.alphabet);
        }
        Debug.Log(currentString);
        if (currentString == currentVoca.voca)
        {
            Debug.Log("정답");
        }
    }
}
