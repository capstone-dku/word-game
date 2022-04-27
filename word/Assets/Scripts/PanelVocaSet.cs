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
        for (int i = 1; i <= count; i++)
        {
            ButtonVocaSet button = Instantiate(prefabButtonVocaSet, scrollViewContent);
            button.textSet.text = i.ToString();
            button.Init();
            button.UpdateDifficulty(difficulty);
            button.UpdateSetNumber(i);

            // TODO: SaveLoad.cs에서 세트 클리어 여부 불러오기 구현 후 파라미터 바꾸기
            bool locked = i <= 3 ? false : true;
            button.UpdateLocked(locked);

            // TODO: SaveLoad.cs의 GetStars(별 갯수 불러오기 기능) 구현 후 함수 바꾸기
            button.UpdateStars(0);
            // 사용자의 정답률만큼 별 갯수 갱신
            //button.UpdateStars(saveLoad.GetStars(diff, i));


        }
    }
}
