using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingBlueprint : MonoBehaviour
{
    public GridMap gridMap;
    public Building buildingObject;
    private bool canBuild = false;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Button buttonBuildOk;
    [SerializeField] private Button buttonBuildCancel;
    private void Awake()
    {
        gridMap = GameObject.Find("Ground").GetComponent<GridMap>();
        buttonBuildOk.onClick.AddListener(OnClickBuild);
        buttonBuildCancel.onClick.AddListener(OnClickCancel);

        UpdateColor();
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

        UpdateColor();
    }

    public void OnClickBuild()
    {
        if (canBuild)
        {
            gridMap.OnBuild(this);

        }
    }

    public void OnClickCancel()
    {
        Destroy(gameObject);
    }

    public void UpdateColor()
    {
        if (gridMap.IsTileEmpty(transform.position))
        {
            canBuild = true;
            spriteRenderer.color = Color.green;
        }
        else
        {
            canBuild = false;
            spriteRenderer.color = Color.red;
        }
    }

    public void UpdateBuilding(Building building)
    {
        buildingObject = building;
        spriteRenderer.sprite = building.spriteRenderer.sprite;
        spriteTransform.position = building.spriteTransform.position;
    }
}
