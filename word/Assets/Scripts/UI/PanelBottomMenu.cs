using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBottomMenu : MonoBehaviour
{
    public Button buttonStudy;
    public Button buttonGame;
    public Button buttonBuilding;
    public Button buttonMission;

    public VocaStudy vocaStudy;
    public VocaGame vocaGame;
    public Mission missionMenu;
    public BuildingManager BuildingManager;
    public void OnClickedStudy()
    {
        vocaStudy.OnButtonClickedStudy();
    }

    public void OnClickedGame()
    {
        vocaGame.OnClickedGame();
    }

    public void OnClickedBuilding()
    {
        BuildingManager.OnClickedBuild();
    }

    public void OnClickedMission()
    {
        missionMenu.OnClickedMission();
    }
}
