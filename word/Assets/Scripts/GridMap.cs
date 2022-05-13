using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    public Tilemap tilemap;
    public SaveLoad saveLoad;
    private TileBase[] allTiles;

    private Dictionary<Vector3, BUILDING> buildings;
    public List<GameObject> buildingPrefabs;
    public Transform gridTransform;
    private int width;
    private int height;

    private void Start()
    {
        Vector3Int size = tilemap.size;
        width = size.x;
        height = size.y;
        buildings = new Dictionary<Vector3, BUILDING>();
        BoundsInt bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(position) != null)
            {
                Debug.Log(tilemap.GetTile(position).name);
                Vector3 cellPosition = tilemap.GetCellCenterWorld(position);
                BUILDING building = saveLoad.GetBuilding(cellPosition);

                // cellPosition = 타일의 좌표값
                // building = 타일에 건설되어있는 건물의 id (BuildingBlueprint의 enum) 
                //
                //
                if (building > 0)
                {
                    if (buildings.ContainsKey(cellPosition))
                    {
                        buildings[cellPosition] = building;
                    }
                    else
                    {
                        buildings.Add(cellPosition, building);
                    }

                    Instantiate(buildingPrefabs[(int)buildings[cellPosition]], cellPosition, new Quaternion(),
                        this.transform);
                }
            }
        }
        /*
        Debug.Log(tilemap.CellToWorld(tilemap.origin));
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = tilemap.CellToWorld(new Vector3Int(i, j, 0));
                Instantiate(buildingPrefab, pos, new Quaternion(), gridTransform);
            }
        }*/
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
        if (buildings[cellPos] != BUILDING.None)
            return false;
        else 
            return true;
    }

    public void OnBuild(BuildingBlueprint bb)
    {
        Instantiate(bb.buildingObject, bb.transform.position, bb.transform.localRotation, this.transform);
        Vector3Int cellPos = tilemap.WorldToCell(bb.transform.position);
        buildings[cellPos] = BUILDING.House;
        Destroy(bb.gameObject);

        saveLoad.Build(cellPos, bb.building);
        saveLoad.SaveData();
    }
}
