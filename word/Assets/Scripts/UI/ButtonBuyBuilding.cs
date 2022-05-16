using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuyBuilding : MonoBehaviour
{
    private PriseList price;
    [SerializeField] private Image imageBuilding;
    [SerializeField] private Text[] textCoins;
    [SerializeField] private Text textName;

    public void UppdateBuilding(PriseList price)
    {
        this.price = price;

        textName.text = price.name;
        // imageBuilding.sprite;
        textCoins[0].text = price.coin0.ToString();
        textCoins[1].text = price.coin1.ToString();
        textCoins[2].text = price.coin2.ToString();
    }
}
