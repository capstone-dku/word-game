using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelQuiz : MonoBehaviour
{
    private int currentIndex = 0;
    private List<Voca> vocaList;

    [SerializeField] private Text textVoca;
    [SerializeField] private Text[] textButton;
    public void Init(List<Voca> vocaList)
    {
        this.vocaList = vocaList;
        this.currentIndex = 0;
    }

    public void StartQuiz()
    {
        List<int> random = new List<int>();
        textVoca.text = vocaList[currentIndex].voca;
        int rnd = Random.Range(0, textButton.Length-1);
        textButton[rnd].text = vocaList[currentIndex].meaning[0];

    }
}
