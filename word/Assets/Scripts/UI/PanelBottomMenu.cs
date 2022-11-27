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

        vocaGame.OnClickedGame(false);
        missionMenu.OnClickedMission(false);
        BuildingManager.gameObject.SetActive(false);
    }

    public void OnClickedGame()
    {
        vocaGame.OnClickedGame(true);

        vocaStudy.OnClose();
        missionMenu.OnClickedMission(false);
        BuildingManager.OnClickedBuild(false);
    }

    public void OnClickedBuilding()
    {
        BuildingManager.OnClickedBuild(true);

        vocaStudy.OnClose();
        vocaGame.OnClickedGame(false);
        missionMenu.OnClickedMission(false);
    }

    public void OnClickedMission()
    {
        missionMenu.OnClickedMission(true);

        vocaStudy.OnClose();
        vocaGame.OnClickedGame(false);
        BuildingManager.OnClickedBuild(false);
    }
}
