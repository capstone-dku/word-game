using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPlayGame : MonoBehaviour
{
    public Button buttonWordPuzzle;
    public VocaSelector vocaSelector;
    public PanelWordPuzzle panelWordPuzzle;
    public PanelCrossWord panelCrossWord;
    public PanelCardGame panelCardGame;
    public PanelAlarm panelAlarm;
    public void OnClickedWordPuzzle()
    {
        if (SaveLoad.Instance.GetTicket() > 0)
        {
            SaveLoad.Instance.AddTicket(-1);
            panelWordPuzzle.gameObject.SetActive(true);

            List<Voca> vocaList = vocaSelector.FindVocaWeight(5);
            Debug.Log(vocaList.Count);
            if (vocaList.Count < 5)
            {
                panelAlarm.gameObject.SetActive(true);
                panelAlarm.SetText("학습을 먼저 해주세요.");
            }
            panelWordPuzzle.Init(vocaList);
            panelWordPuzzle.StartGame();

            gameObject.SetActive(false);
            SaveLoad.Instance.SaveData();
        }
        else
        {
            panelAlarm.gameObject.SetActive(true);
            panelAlarm.SetText("티켓이 부족합니다.");
        }
    }

    public void OnClickedCrossWord()
    {
        if (SaveLoad.Instance.GetTicket() > 0)
        {
            SaveLoad.Instance.AddTicket(-1);
            panelCrossWord.gameObject.SetActive(true);
            gameObject.SetActive(false);

            List<Voca> vocaList = vocaSelector.FindVocaWeight(5);
            Debug.Log(vocaList.Count);
            if (vocaList.Count < 5)
            {
                panelAlarm.gameObject.SetActive(true);
                panelAlarm.SetText("학습을 먼저 해주세요.");
            }
            panelCrossWord.Init(vocaList);
            panelCrossWord.StartGame();
            
            SaveLoad.Instance.SaveData();
        }
        else
        {
            panelAlarm.gameObject.SetActive(true);
            panelAlarm.SetText("티켓이 부족합니다.");
        }
    }
    public void OnClickedCardGame()
    {
        if (SaveLoad.Instance.GetTicket() > 0)
        {
            SaveLoad.Instance.AddTicket(-1);
            panelCardGame.gameObject.SetActive(true);
            gameObject.SetActive(false);

            List<Voca> vocaList = vocaSelector.FindVocaWeight(10);
            Debug.Log(vocaList.Count);
            if (vocaList.Count < 10)
            {
                panelAlarm.gameObject.SetActive(true);
                panelAlarm.SetText("학습을 먼저 해주세요.");
            }
            panelCardGame.Init(vocaList);
            panelCardGame.StartGame();

            SaveLoad.Instance.SaveData();
        }
        else
        {
            panelAlarm.gameObject.SetActive(true);
            panelAlarm.SetText("티켓이 부족합니다.");
        }
    }
}
