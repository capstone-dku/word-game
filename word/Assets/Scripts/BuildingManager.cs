using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public enum BUILDING
{
    None,
    School,
    WordPuzzle,
    CrossWord,
    WordCard,

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
    [Header("list와 enum의 인덱스를 똑같이 맞춰주세요")]
    public BUILDING building;
    public List<GameObject> buildingPrefabs;

    public PanelShop panelShop;
    // Start is called before the first frame update
    public GameObject GetPrefab(BUILDING building)
    {
        return buildingPrefabs[(int)building];
    }

    public void OnClickedBuild()
    {
        panelShop.gameObject.SetActive(true);
        panelShop.Init();
        panelShop.UpdateButton();

    }
}
