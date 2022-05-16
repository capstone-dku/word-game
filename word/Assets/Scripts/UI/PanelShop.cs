using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShop : MonoBehaviour
{

    [SerializeField] private ShopManager shopManager;
    public ButtonBuyBuilding buttonBuyBuilding;
    public void Init()
    {
        shopManager.GetPriseList();
        for (int i = 0; i<shopManager.priseList.Count; i++)
        {
            PriseList price = shopManager.priseList[i];
            buttonBuyBuilding.UppdateBuilding(price);
        }
    }
}
