using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class VocaSelector : MonoBehaviour
{    
    // TODO: 특정 difficulty, level의 단어를 단어장 json 파일에서 가져온다.
    public List<Voca> SelectVoca(int difficulty, int level)
    {
        List<Voca> voca = JsonLoad(difficulty).GetRange(level*20,20);
        
        return voca;
    }
    
    /// <summary>
    /// 특정 diffculty의 set 갯수를 반환한다.
    /// </summary>
    public int GetVocaSetCount(int difficulty)
    {
        int count = 0;
        

        return count;
    }

    public List<Voca> JsonLoad(int difficulty)
    {
        List<Voca> __voca;
        string jdata;
        switch(difficulty){
            case 0:
                jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/Beginner.json");
                break;
            case 1:
                jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/Intermediate.json");
                break;
            case 2:
                jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/Advanced.json");
                break;
            case 3:
                jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/Toeic.json");
                break;
            default:
                jdata = "";
                break;
        }
        __voca = JsonConvert.DeserializeObject<List<Voca>>(jdata);
        return __voca;
    }
}
