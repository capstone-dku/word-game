using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingBlueprint : MonoBehaviour
{
    public GridMap gridMap;
    public Building buildingObject;
    public BUILDING building;
    [SerializeField] private Button buttonBuildOk;
    [SerializeField] private Button buttonBuildCancel;
    private void Awake()
    {
        gridMap = GameObject.Find("Ground").GetComponent<GridMap>();
        buttonBuildOk.onClick.AddListener(OnClickBuild);
    }

    private void OnMouseDrag()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;
        //Vector3 pos = gridMap.GetNearTilePosition(worldPos);
        transform.position = worldPos;
    }

    private void OnMouseUp()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;
        Vector3 pos = gridMap.GetNearTilePosition(worldPos);
        transform.position = pos;
    }

    public void OnClickBuild()
    {
        gridMap.OnBuild(this);
    }
}
