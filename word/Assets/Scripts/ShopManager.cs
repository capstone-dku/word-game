using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class PriseList{ // 가격 리스트. id는 임의의 값이아니라
    public int id;
    public string name;
    public int coin0;
    public int coin1;
    public int coin2;
    public int unlock;
    public int type;
}

public class ShopManager : MonoBehaviour
{
    public List<PriseList> priseList; //[id][coin_type]
    private bool[] unlockList;

    private void Start()
    {
        GetPriseList();
        Init(new bool[priseList.Count]);
    }
    /// 생성시 초기화 ///
    public void Init(bool[] ul){
        // 세이브데이터 불러오기
        unlockList = ul;
        for (int i = 0; i < unlockList.Length; i++)
        {
            unlockList[i] = !Convert.ToBoolean(priseList[i].unlock);
        }
    }
    ///
    
    /// 가격 리스트 불러오기 ///
    public void GetPriseList(){
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/ShopList.json");
        priseList = JsonConvert.DeserializeObject<List<PriseList>>(jdata);
        /*
        for (int i = 0; i < priseList.Count; i++)
        {
            Debug.Log(priseList[i].id);
            Debug.Log(priseList[i].name);
            Debug.Log(priseList[i].coin0);
            Debug.Log(priseList[i].coin1);
            Debug.Log(priseList[i].coin2);
            Debug.Log(priseList[i].unlock);
            Debug.Log(priseList[i].type);
        }
        */
    }

    ///

    /// 구매 성공 및 실패시 반환///
    public bool BuyBuilding(int id)
    {
        if (unlockList[id] == false) return false;
        if (priseList[id].coin0 > SaveLoad.Instance.GetCoin(0)) return false;
        if (priseList[id].coin1 > SaveLoad.Instance.GetCoin(1)) return false;
        if (priseList[id].coin2 > SaveLoad.Instance.GetCoin(2)) return false;
        SaveLoad.Instance.AdjustCoin(0, -priseList[id].coin0);
        SaveLoad.Instance.AdjustCoin(1, -priseList[id].coin1);
        SaveLoad.Instance.AdjustCoin(2, -priseList[id].coin2);
        return true;
    }

    /// 상점에 건물을 언락함 ///
    public void Unlock(int id){
        unlockList[id] = true;
    }
    ///

    /// 분류별로 리스트를 불러옴 ///
    public List<PriseList> GetPriseListType(int type){
        List<PriseList> pl = new List<PriseList>();
        for(int i=0;i<priseList.Count;i++){
            if(priseList[i].type == type)
            {
                pl.Add(priseList[i]);
            }
        }
        return pl;
    }
    ///
}
