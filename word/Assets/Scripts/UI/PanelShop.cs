using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelShop : MonoBehaviour
{

    [SerializeField] private ShopManager shopManager;
    [SerializeField] private Transform transformContent;
    public ButtonBuyBuilding buttonBuyBuildingPrefab;
    private List<ButtonBuyBuilding> buttons = new List<ButtonBuyBuilding>();
    

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        shopManager.GetPriseList();
        for (int i = 0; i<shopManager.priseList.Count; i++)
        {
            PriseList price = shopManager.priseList[i];
            ButtonBuyBuilding button = Instantiate(buttonBuyBuildingPrefab, transformContent);
            button.UppdateBuilding(price);
            buttons.Add(button);
        }
    }

    public void UpdateButton()
    {
        for (int i = 0; i < shopManager.priseList.Count; i++)
        {
            buttons[i].UppdateBuilding(shopManager.priseList[i]);
        }
    }
}
