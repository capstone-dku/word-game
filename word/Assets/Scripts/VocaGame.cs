using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocaGame : MonoBehaviour
{

    public PanelPlayGame panelPlayGame;
    public void OnClickedGame(bool active)
    {
        panelPlayGame.gameObject.SetActive(active);
    }

    public void OnClickedClose()
    {
        panelPlayGame.gameObject.SetActive(false);
    }
}
