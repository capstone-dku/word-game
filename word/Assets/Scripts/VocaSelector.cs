using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class VocaSelector : MonoBehaviour
{    
    const int BEGINNER = 800;
    const int INTERMEDIATE = 1800;
    const int ADVANCED = 400;
    const int TOEIC = 2660;
    private const int VOCA_PER_SET = 20;

    private List<Voca> vocaList = null;

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
        switch (difficulty)
        {
            case 0: 
                return BEGINNER / VOCA_PER_SET;
            case 1:
                return INTERMEDIATE / VOCA_PER_SET;
            case 2:
                return ADVANCED / VOCA_PER_SET;
            case 3:
                return TOEIC / VOCA_PER_SET;
            default:
                return 0;
        }
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
    /// <summary>
    /// 티켓파일 초기화 파일생성해주지만 만들어진 상태로 파일 넣어주는게 나을것 같아요.
    /// </summary>
    public void InitVocaTicket(){
        List<VocaTicket> vtlist = new List<VocaTicket>();
        for(int i=0;i<BEGINNER/20;i++){
            VocaTicket vt = new VocaTicket(0);
            vtlist.Add(vt);
        }
        for(int i=0;i<INTERMEDIATE/20;i++){
            VocaTicket vt = new VocaTicket(0);
            vtlist.Add(vt);
        }
        for(int i=0;i<ADVANCED/20;i++){
            VocaTicket vt = new VocaTicket(0);
            vtlist.Add(vt);
        }
        for(int i=0;i<TOEIC/20;i++){
            VocaTicket vt = new VocaTicket(0);
            vtlist.Add(vt);
        }
        VocaTicketMeta vtm = new VocaTicketMeta(BEGINNER,INTERMEDIATE,ADVANCED,TOEIC);
        string jdata = JsonConvert.SerializeObject(vtlist);
        File.WriteAllText(Application.dataPath + "/VocaTicket.json", jdata);
        jdata = JsonConvert.SerializeObject(vtm);
        File.WriteAllText(Application.dataPath + "/VocaTicketMeta.json", jdata);
    }

    /// <summary>
    /// 학습한 단어를 티켓과 함께 저장
    /// </summary>
    public void AddVocaTicket(int difficulty, int level)
    {
        List<VocaTicket> vtlist;
        VocaTicketMeta vtm;
        if(!File.Exists(Application.dataPath + "/VocaTicket.json")){
            InitVocaTicket();
        }

        string jdata = File.ReadAllText(Application.dataPath + "/VocaTicket.json");
        vtlist = JsonConvert.DeserializeObject<List<VocaTicket>>(jdata);
        jdata = File.ReadAllText(Application.dataPath + "/VocaTicketMeta.json");
        vtm = JsonConvert.DeserializeObject<VocaTicketMeta>(jdata);
        //불러오기
        
        VocaTicket vt = new VocaTicket(10);
        vtm.sum+=200;
        
        jdata = JsonConvert.SerializeObject(vtlist);
        File.WriteAllText(Application.dataPath + "/VocaTicket.json", jdata);
        jdata = JsonConvert.SerializeObject(vtm);
        File.WriteAllText(Application.dataPath + "/VocaTicketMeta.json", jdata);
        //저장
    }

    /// <summary>
    /// 학습한 단어를 찾음
    /// </summary>
    public List<Voca> FindVocaTicket(int num)
    {
        List<VocaTicket> vtlist;
        VocaTicketMeta vtm;
        string jdata = File.ReadAllText(Application.dataPath + "/VocaTicket.json");
        vtlist = JsonConvert.DeserializeObject<List<VocaTicket>>(jdata);
        jdata = File.ReadAllText(Application.dataPath + "/VocaTicketMeta.json");
        vtm = JsonConvert.DeserializeObject<VocaTicketMeta>(jdata);

        List<Voca> voca = new List<Voca>();
        for(int i=0;i<num;i++){
            int t = Random.Range(0, vtm.sum-1);

        }
        return voca;
    }

    /// <summary>
    /// 단어에 새로운 티켓을 부여함.
    /// </summary>
    public void SaveVocaTicket()
    {
        
    }
}
