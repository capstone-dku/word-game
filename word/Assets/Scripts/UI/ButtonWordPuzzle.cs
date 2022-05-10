using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWordPuzzle : MonoBehaviour
{
    public int index;
    public bool clicked; // 해당 인덱스 버튼의 클릭 여부
    public ALPHABET alphabet; // 해당 인덱스 버튼의 알파벳
    [SerializeField] private Image image;

    public void Init(int index)
    {
        this.index = index;
        this.alphabet = ALPHABET.Empty;
        this.clicked = false;
        this.image.color = Color.white;
    }

    public void UpdateSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }
    
}
