using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelWordPuzzle : MonoBehaviour
{
    [SerializeField] private VocaSelector vocaSelector;
    public Button[] buttonAlphabet;

    public List<Sprite[]> sprites = new List<Sprite[]>();
    public Sprite[] spriteRed;
    public Sprite[] spriteBlue;
    public Sprite[] spritePurple;
    public Sprite[] spriteGreen;
    public Sprite[] spriteGrey;

    private bool[] clicked; // 해당 인덱스 버튼의 클릭 여부
    private char[] alphabets; // 해당 인덱스 버튼의 알파벳
    private Text textRemainTime;
    private Text textMeaingWord;
    private List<Voca> vocaList;
    private Voca currentVoca;

    private int time;

    private void Start()
    {
        clicked = new bool[buttonAlphabet.Length];
        sprites.Add(spriteRed);
        sprites.Add(spriteBlue);
        sprites.Add(spritePurple);
        sprites.Add(spriteGreen);
        sprites.Add(spriteGrey);

        Init(vocaSelector.FindVocaWeight(1));
    }

    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        time = 180;
        StartPuzzle();
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

    public void OnButtonClickAlphabet(int idx)
    {

    }

}
