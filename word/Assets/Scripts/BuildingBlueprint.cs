using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingBlueprint : MonoBehaviour
{
    public GridMap gridMap;
    public Building buildingObject;
    public Sprite buildingSprite;
    public BUILDING building;
    private bool canBuild = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Button buttonBuildOk;
    [SerializeField] private Button buttonBuildCancel;
    private void Awake()
    {
        gridMap = GameObject.Find("Ground").GetComponent<GridMap>();
        buttonBuildOk.onClick.AddListener(OnClickBuild);
        buttonBuildCancel.onClick.AddListener(OnClickCancel);
        buildingSprite = spriteRenderer.sprite;
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

        if (gridMap.IsTileEmpty(pos))
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
}
