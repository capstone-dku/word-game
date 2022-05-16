using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    public Tilemap tilemap;
    public SaveLoad saveLoad;
    private TileBase[] allTiles;


    private List<BuildingData> buildingData;
    public Transform gridTransform;
    private int width;
    private int height;

    [SerializeField]private BuildingManager buildingManager;

    private void Start()
    {
        Vector3Int size = tilemap.size;
        width = size.x;
        height = size.y;
        buildingData = saveLoad.GetBuildingData();

        for (int i = 0; i < buildingData.Count; i++)
        {
            Vector3 pos = new Vector3(buildingData[i].x, buildingData[i].y, buildingData[i].z);
            GameObject prefab = buildingManager.GetPrefab((int)buildingData[i].building);
            Instantiate(prefab, pos, new Quaternion(), this.transform);
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
