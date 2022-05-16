using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuyBuilding : MonoBehaviour
{
    private PriseList price;
    private BuildingManager buildingManager;
    [SerializeField] private Image imageBuilding;
    [SerializeField] private Text[] textCoins;
    [SerializeField] private Text textName;
    [SerializeField] private GameObject panelLocked;

    public void Init()
    {
        buildingManager = GameObject.Find("Building Manager").GetComponent<BuildingManager>();
        Debug.Log(buildingManager);
    }
    public void UppdateBuilding(PriseList price)
    {
        this.price = price;

        textName.text = price.name;
        // imageBuilding.sprite;
        textCoins[0].text = price.coin0.ToString();
        textCoins[1].text = price.coin1.ToString();
        textCoins[2].text = price.coin2.ToString();

        if (price.unlock == 0)
        {
            panelLocked.SetActive(false);
        }
        else
        {
            panelLocked.SetActive(true);
        }

        
        GameObject go = buildingManager.GetPrefab((BUILDING)price.id);
        imageBuilding.sprite = go.GetComponentInChildren<SpriteRenderer>().sprite;
    }
}
