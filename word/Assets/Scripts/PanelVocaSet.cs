using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelVocaSet : MonoBehaviour
{
    [SerializeField] private Transform scrollViewContent; // 세트 선택 창 스크롤뷰 컨텐츠
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField][Range(0,3)] private int difficulty;
    [SerializeField] private ButtonVocaSet prefabButtonVocaSet; // 세트 버튼 프리팹


    public void InitPanel()
    {
        // 패널에 단어 세트만큼 버튼을 추가한다.
        int count = vocaSelector.GetVocaSetCount(difficulty);
        for (int i = 0; i < 4/*TODO: 테스트 후 count로 바꾸기*/; i++)
        {
            ButtonVocaSet button = Instantiate(prefabButtonVocaSet, scrollViewContent);
            button.textSet.text = (i + 1).ToString();
            button.UpdateStars(2);
            // 사용자가 했던것만큼 
            //button.UpdateStars(saveLoad.GetStars(diff, i + 1));


        }
    }
}
