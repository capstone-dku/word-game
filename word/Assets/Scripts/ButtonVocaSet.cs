using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVocaSet : MonoBehaviour
{
    [SerializeField] private VocaStudy vocaStudy;
    public Text textSet;
    public GameObject[] emptyStars = new GameObject[3];
    public GameObject[] fullStars = new GameObject[3];
    [SerializeField] private GameObject objectLocked;
    private int setNumber = -1;
    private int difficulty = -1;
    private int star = 0;
    private bool locked = false;

    public void Init()
    {
        vocaStudy = GameObject.Find("단어 학습").GetComponent<VocaStudy>();
    }
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

        this.star = star;
    }

    public void UpdateSetNumber(int num)
    {
        this.setNumber = num;
    }
    public void UpdateDifficulty(int diff)
    {
        this.difficulty = diff;
    }
    public void UpdateLocked(bool locked)
    {
        this.locked = locked;
        if (locked)
        {
            objectLocked.SetActive(true);
        }
        else
        {
            objectLocked.SetActive(false);
        }
    }
    public void OnButtonClicked()
    {
        if (locked == false)
        {
            vocaStudy.StudyConfirm(difficulty, setNumber, star);
        }
        
    }
    

}
