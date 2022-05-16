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
    private List<PriseList> priseList; //[id][coin_type]
    private bool[] unlockList;

    /// 생성시 초기화 ///
    public void Init(bool[] ul){
        unlockList = ul;
    }
    ///

    /// 가격 리스트 불러오기 ///
    public void GetPriseList(){
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/ShopList.json");
        priseList = JsonConvert.DeserializeObject<List<PriseList>>(jdata);
    }
    ///
    
    /// 구매 성공 및 실패시 반환///
    public bool BuyBuilding(int id, int[] coin){
        if((unlockList[id] == true) && (priseList[id].coin0 <= SaveLoad.Instance.GetCoin(0)) && (priseList[id].coin1 <= SaveLoad.Instance.GetCoin(1)) && (priseList[id].coin1) <= SaveLoad.Instance.GetCoin(2)){
            SaveLoad.Instance.AdjustCoin(0, -priseList[id].coin0);
            SaveLoad.Instance.AdjustCoin(1, -priseList[id].coin1);
            SaveLoad.Instance.AdjustCoin(2, -priseList[id].coin2);
            return true;
        }
        return false;
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
