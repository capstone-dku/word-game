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
    [SerializeField] private Button button;
    public void Init()
    {
        buildingManager = GameObject.Find("Building Manager").GetComponent<BuildingManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
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
        Building go = buildingManager.GetObjectPrefab(price.id);
        imageBuilding.sprite = go.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void OnClicked()
    {
        buildingManager.OnClickedButton(price.id);
    }
}
