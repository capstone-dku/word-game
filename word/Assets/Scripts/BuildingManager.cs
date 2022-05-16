using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public enum BUILDING
{
    None,
    House,
    School,
    APT,
}
[System.Serializable]
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
public class BuildingManager : MonoBehaviour
{
    public List<GameObject> buildingPrefabs;
    // Start is called before the first frame update
    public GameObject GetPrefab(int index)
    {
        return buildingPrefabs[index];
    }
}
