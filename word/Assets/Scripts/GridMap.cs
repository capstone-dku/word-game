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

    private TileBase[] allTiles;

    public int[,] buildingMap;
    public GameObject buildingPrefab;
    public Transform gridTransform;
    private int width;
    private int height;

    private void Start()
    {
        Vector3Int size = tilemap.size;
        width = size.x;
        height = size.y;
        buildingMap = new int[width, height];
        BoundsInt bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);
        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
            {
                TileBase tile = allTiles[i + j * bounds.size.x];
                if (tile != null)
                {
                    buildingMap[i, j] = 0;
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
        if (buildingMap[cellPos.x, cellPos.y] != 0)
            return false;
        else 
            return false;
    }

    public void OnBuild(BuildingBlueprint bb)
    {
        Instantiate(bb.buildingObject, bb.transform.position, bb.transform.localRotation, this.transform);
        Vector3Int cellPos = tilemap.WorldToCell(bb.transform.position);
        buildingMap[cellPos.x, cellPos.y] = 1;
        Destroy(bb.gameObject);
    }
}
