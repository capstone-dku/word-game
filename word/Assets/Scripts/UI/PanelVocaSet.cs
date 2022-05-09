using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelVocaSet : MonoBehaviour
{
    [SerializeField] private SaveLoad saveLoad;
    [SerializeField] private Transform scrollViewContent; // 세트 선택 창 스크롤뷰 컨텐츠
    [SerializeField] private VocaSelector vocaSelector;
    [SerializeField][Range(0,3)] private int difficulty;
    [SerializeField] private ButtonVocaSet prefabButtonVocaSet; // 세트 버튼 프리팹

    private List<ButtonVocaSet> buttonVocaSets = new List<ButtonVocaSet>();

    public void InitPanel()
    {
        // 패널에 단어 세트만큼 버튼을 추가한다.
        int count = vocaSelector.GetVocaSetCount(difficulty);
        for (int i = 0; i < count; i++)
        {
            ButtonVocaSet button = Instantiate(prefabButtonVocaSet, scrollViewContent);
            button.textSet.text = (i+1).ToString();
            button.Init();
            button.UpdateDifficulty(difficulty);
            button.UpdateSetNumber(i);
            
            bool locked = saveLoad.IsVocaSetLocked(difficulty, i);
            button.UpdateLocked(locked);

            // 사용자의 정답률만큼 별 갯수 갱신
            button.UpdateStars(saveLoad.GetStars(difficulty, i));

            buttonVocaSets.Add(button);
        }
    }

    public void UpdateButtonStars(int setNumber)
    {
        buttonVocaSets[setNumber].UpdateStars(saveLoad.GetStars(this.difficulty, setNumber));
    }

    public void UpdateButtonLocked(int setNumber)
    {
        buttonVocaSets[setNumber].UpdateLocked(saveLoad.IsVocaSetLocked(this.difficulty, setNumber));
    }
}
