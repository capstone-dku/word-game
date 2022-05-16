using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public enum BUILDING
{
    None,
    House,
    School,
    APT,
}
[Serializable]
public class BuildingData
{
    public float x;
    public float y;
    public float z;
    public BUILDING building;

    public BuildingData()
    {
        this.x = 0;
        this.y = 0;
        this.z = 0;
        building = BUILDING.None;
    }

    public BuildingData(Vector3 position, BUILDING building)
    {
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
        this.building = building;
    }
}
public class GridMap : MonoBehaviour
{
    public Tilemap tilemap;
    public SaveLoad saveLoad;
    private TileBase[] allTiles;


    private List<BuildingData> buildingData;
    public List<GameObject> buildingPrefabs;
    public Transform gridTransform;
    private int width;
    private int height;

    private void Start()
    {
        Vector3Int size = tilemap.size;
        width = size.x;
        height = size.y;
        buildingData = saveLoad.GetBuildingData();

        for (int i = 0; i < buildingData.Count; i++)
        {
            Vector3 pos = new Vector3(buildingData[i].x, buildingData[i].y, buildingData[i].z);
            Instantiate(buildingPrefabs[(int)buildingData[i].building], pos, new Quaternion(), this.transform);
        }
    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int posInt = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
           
            TileBase tile = tilemap.GetTile(posInt);
            Vector3 pos = tilemap.CellToWorld(posInt);
            pos.z = 0;
            Instantiate(buildingPrefab, pos, new Quaternion(), gridTransform);

        }
        */
    }

    public Vector3 GetNearTilePosition(Vector3 pos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(pos);
        cellPos.x += 1;
        cellPos.y += 1;
        TileBase tile = tilemap.GetTile(cellPos);
        Vector3 worldPos = tilemap.CellToWorld(cellPos);
        return worldPos;
    }

    public bool IsTileEmpty(Vector3 pos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(pos);
        cellPos.x += 1;
        cellPos.y += 1;
        for (int i = 0; i < buildingData.Count; i++)
        {
            Vector3 v = new Vector3(buildingData[i].x, buildingData[i].y, buildingData[i].z);
            if (pos.Equals(v))
                return false;
        }
        return true;
    }

    public void OnBuild(BuildingBlueprint bb)
    {
        Instantiate(bb.buildingObject, bb.transform.position, bb.transform.localRotation, this.transform);
        Vector3Int cellPos = tilemap.WorldToCell(bb.transform.position);
        BuildingData bd = new BuildingData(bb.transform.position, bb.building);
        buildingData.Add(bd);
        Destroy(bb.gameObject);

        saveLoad.Build(bd);
        saveLoad.SaveData();
    }
}
