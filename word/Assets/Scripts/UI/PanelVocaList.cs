using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelVocaList : MonoBehaviour
{
    [SerializeField] private Transform transformVocaListContent;
    [SerializeField] private PanelVoca panelVoca; // 보카 패널 창
    private List<Voca> vocaList = null;
    private List<GameObject> instances = new List<GameObject>();
    public void Init(List<Voca> vocaList, bool[] answer)
    {
        this.vocaList = vocaList;
        for (int i = 0; i < vocaList.Count; i++)
        {
            Debug.Log(answer[i]);
            PanelVoca pv = Instantiate(panelVoca, transformVocaListContent);
            pv.UpdatePanel(vocaList[i], answer[i]);
            instances.Add(pv.gameObject);
        }
    }

    public void OnButtonClickedClose()
    {
        for (int i = 0; i < instances.Count; i++)
        {
            Destroy(instances[i]);
        }

        instances.Clear();

        gameObject.SetActive(false);

    }
}
