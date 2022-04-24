using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelStudyConfirm : MonoBehaviour
{
    [SerializeField] private VocaStudy vocaStudy;
    [SerializeField] private Text textSetNumber;
    public GameObject[] emptyStars = new GameObject[3];
    public GameObject[] fullStars = new GameObject[3];
    private int setNumber = 0;
    private int difficulty = 0;

    public void UpdateStars(int star)
    {
        for (int i = 0; i < 3; i++)
        {
            fullStars[i].SetActive(false);
            emptyStars[i].SetActive(true);
        }
        for (int i = 0; i < star; i++)
        {
            fullStars[i].SetActive(true);
            emptyStars[i].SetActive(false);
        }
    }

    public void UpdateDifficulty(int diff)
    {
        this.difficulty = diff;
    }
    public void UpdateSetNumber(int num)
    {
        setNumber = num;
        textSetNumber.text = num.ToString();
    }

    public void OnButtonClickedConfirm()
    {
        vocaStudy.Study(difficulty, setNumber);
    }
}
