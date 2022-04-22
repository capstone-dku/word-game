using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVocaSet : MonoBehaviour
{
    public Text textSet;
    public GameObject[] emptyStars = new GameObject[3];
    public GameObject[] fullStars = new GameObject[3];

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

    public void OnButtonClicked()
    {

    }

}
