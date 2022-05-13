using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelItem : MonoBehaviour
{
    public Text textTicket;
    public Text[] textCoin;

    public void UpdateTicket(int ticket)
    {
        textTicket.text = ticket.ToString();
    }

    public void UpdateCoin(int type, int coin)
    {
        textCoin[type].text = coin.ToString();
    }
}
