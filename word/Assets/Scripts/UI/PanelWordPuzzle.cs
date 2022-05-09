using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ALPHABET
{
    A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,Length, Empty
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
        new Tuple<int, int>(1,0),  // UP
        new Tuple<int, int>(0,1),  // RIGHT
        new Tuple<int, int>(0,-1), // LEFT
        new Tuple<int, int>(-1,0)  // DOWN
    };
    
    private const int WIDTH = 4;
    private const int HEIGHT = 6;
    [SerializeField] private Text textRemainTime;
    [SerializeField] private Text textMeaingWord;
    private List<Voca> vocaList;
    private Voca currentVoca;
    private bool complete = false;
    private int time;

    private void Start()
    {
        for (int i = 0; i < HEIGHT * WIDTH; i++)
        {
            buttonWordPuzzles[i].Init(i);
        }
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
        int length = voca.voca.Length;
        Debug.Log(voca.voca.ToString() + "퍼즐 만들기");

        int y = Random.Range(0, HEIGHT);
        int x = Random.Range(0, WIDTH);
        PlaceAlphabet(voca.voca, y, x, 0, length);
        /*
        for (int i = 0; i < WIDTH * HEIGHT; i++)
        {
            if(buttonWordPuzzles[i].alphabet == ALPHABET.Empty)
                buttonWordPuzzles[i].alphabet = (ALPHABET)Random.Range(0, (int)ALPHABET.Length);
        }
        */
        UpdateSprites();
    }

    private void PlaceAlphabet(string voca, int y, int x, int depth, int length)
    {
        if (depth >= length)
        {
            complete = true;
            Debug.Log("단어 완성");
            return;
        }
        

        // 랜덤으로 다음 위치 선정
        List<int> random = new List<int>() { 0, 1, 2, 3 };
        int ny = y, nx = x;
        while (random.Count > 0)
        {
            int rnd = Random.Range(0, random.Count);
            Tuple<int, int> next = direction[rnd];
            random.RemoveAt(rnd);
            ny = y + next.Item1;
            nx = x + next.Item2;
            ALPHABET alphabet = (ALPHABET)Enum.Parse(typeof(ALPHABET), char.ToUpper(voca[depth]).ToString());
            buttonWordPuzzles[y * WIDTH + x].alphabet = alphabet;
            if (CanPlaceAlphabet(ny, nx) && complete == false)
                PlaceAlphabet(voca, ny, nx, depth+1, length);
            else
                buttonWordPuzzles[y * WIDTH + x].alphabet = ALPHABET.Empty;
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

    }
}
