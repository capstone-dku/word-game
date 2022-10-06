using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonWordPuzzle : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public int index;
    public bool clicked; // 해당 인덱스 버튼의 클릭 여부
    public ALPHABET alphabet; // 해당 인덱스 버튼의 알파벳
    [SerializeField] private Image image;
    private PanelWordPuzzle pwp;

    public void Init(int index, PanelWordPuzzle pwp)
    {
        this.index = index;
        this.alphabet = ALPHABET.Empty;
        this.clicked = false;
        this.pwp = pwp;
    }

    public void UpdateSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void OnPointerDown(PointerEventData eventData){
        //Debug.Log("OnpointerEnter");
        //print(index.ToString());
        pwp.OnButtonClickAlphabet(this);
    }  
    //
    public void OnPointerEnter(PointerEventData eventData){
        //Debug.Log("OnpointerEnter");
        //print(index.ToString());
        pwp.OnButtonDragAlphabet(this);
    }   
    
}
