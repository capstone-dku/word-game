using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocaGame : MonoBehaviour
{

    public PanelPlayGame panelPlayGame;
    public void OnClickedGame()
    {
        panelPlayGame.gameObject.SetActive(true);
    }

    public void OnClickedClose()
    {
        panelPlayGame.gameObject.SetActive(false);
    }
}
