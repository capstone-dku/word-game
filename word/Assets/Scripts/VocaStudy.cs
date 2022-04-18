using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocaStudy : MonoBehaviour
{
    [SerializeField]
    private GameObject difficultyPanel; // 난이도 선택 패널창


    void StartStudy()
    {
        difficultyPanel.SetActive(true);
        
    }

    void SelectDiffculty()
    {

    }
}
