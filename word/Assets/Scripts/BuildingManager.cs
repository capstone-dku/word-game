using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public float x;
    public float y;
    public float z;
    public int id;

    public BuildingData()
    {
        this.x = 0;
        this.y = 0;
        this.z = 0;
        this.id = 0;
    }

    public BuildingData(Vector3 position, int id)
    {
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
        this.id = id;
    }
}
public class BuildingManager : MonoBehaviour
{
    public List<Building> buildingPrefabs;
    public BuildingBlueprint blueprintPrefabs;
    public ShopManager shopManager;

    public PanelShop panelShop;
    
    // Start is called before the first frame update
    private void Start()
    {
        panelShop.Init();
    }
    public Building GetObjectPrefab(int id)
    {
        for (int i = 0; i < buildingPrefabs.Count; i++)
        {
            if (id == buildingPrefabs[i].id)
                return buildingPrefabs[i];
        }
        return null;
    }
    
    public void OnClickedBuild()
    {
        panelShop.gameObject.SetActive(true);
        panelShop.UpdateButton();
    }

    public void OnClickedButton(int id)
    {
        if (shopManager.BuyBuilding(id))
        {
            Debug.Log("true");
            panelShop.gameObject.SetActive(false);
            BuildingBlueprint bb = Instantiate(blueprintPrefabs);
            Building building = GetObjectPrefab(id);

            bb.UpdateBuilding(building);
        }
        else
        {
            Debug.Log("false");

        }
        
    }
}
