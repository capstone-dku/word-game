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

    const int VOCA_PER_SET = 20;
    
    public List<Voca> beginnerVoca = null;
    public List<Voca> intermediateVoca = null;
    public List<Voca> advancedVoca = null;
    public List<Voca> toeicVoca = null;

    public List<VocaTicket> vtlist = null;
    public List<VocaTicketMeta> vtm = null;

    // TODO: 특정 difficulty, level의 단어를 단어장 json 파일에서 가져온다.

    public List<Voca> SelectVoca(int difficulty, int level)
    {
        List<Voca> voca;
        if(beginnerVoca == null){
            JsonLoad();
        }
        switch(difficulty){
            case 0:
                voca = beginnerVoca.GetRange(level*20,20);
                break;
            case 1:
                voca = intermediateVoca.GetRange(level*20,20);
                break;
            case 2:
                voca = advancedVoca.GetRange(level*20,20);
                break;
            case 3:
                voca = toeicVoca.GetRange(level*20,20);
                break;
            default:
                voca = null;
                break;
        }
        
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

    public void JsonLoad()
    {
        string jdata;
        jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/Beginner.json");
        beginnerVoca = JsonConvert.DeserializeObject<List<Voca>>(jdata);
        jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/Intermediate.json");
        intermediateVoca = JsonConvert.DeserializeObject<List<Voca>>(jdata);
        jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/Advanced.json");
        advancedVoca = JsonConvert.DeserializeObject<List<Voca>>(jdata);
        jdata = File.ReadAllText(Application.dataPath + "/Resources/JsonData/Toeic.json");
        toeicVoca = JsonConvert.DeserializeObject<List<Voca>>(jdata);
    }
    /// <summary>
    /// 티켓파일 초기화 파일생성해주지만 만들어진 상태로 파일 넣어주는게 나을것 같아요.
    /// </summary>
    public void InitVocaTicket(){
        vtlist = new List<VocaTicket>();
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
        vtm = new VocaTicketMeta(BEGINNER,INTERMEDIATE,ADVANCED,TOEIC);
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
        if(!File.Exists(Application.dataPath + "/VocaTicket.json")){
            InitVocaTicket();
        }
        if(vtlist == null){
            string jdata = File.ReadAllText(Application.dataPath + "/VocaTicket.json");
            vtlist = JsonConvert.DeserializeObject<List<VocaTicket>>(jdata);
            jdata = File.ReadAllText(Application.dataPath + "/VocaTicketMeta.json");
            vtm = JsonConvert.DeserializeObject<VocaTicketMeta>(jdata);
        }
        //불러오기
        
        VocaTicket vt = new VocaTicket(10);
        int tempSum = 0;
        
        switch(difficulty){
            case 0:
                tempSum = vtlist[level].sum;
                vtlist[level] = vt;
                break;
            case 1:
                tempSum = vtlist[level+((BEGINNER)/20)].sum;
                vtlist[level+((BEGINNER)/20)] = vt;
                break;
            case 2:
                tempSum = vtlist[level+((BEGINNER+INTERMEDIATE)/20)].sum;
                vtlist[level+((BEGINNER+INTERMEDIATE)/20)] = vt;
                break;
            case 3:
                tempSum = vtlist[level+((BEGINNER+INTERMEDIATE+ADVANCED)/20)].sum;
                vtlist[level+((BEGINNER+INTERMEDIATE+ADVANCED)/20)] = vt;
                break;
        }

        vtm.sum += 200 - tempSum;
        
        jdata = JsonConvert.SerializeObject(vtlist);
        File.WriteAllText(Application.dataPath + "/VocaTicket.json", jdata);
        jdata = JsonConvert.SerializeObject(vtm);
        File.WriteAllText(Application.dataPath + "/VocaTicketMeta.json", jdata);
        //저장
    }

    /// <summary>
    /// 학습한 단어를 티켓에 비례하여 랜덤으로 찾음.
    /// </summary>
    public List<Voca> FindVocaTicket(int num) // 몇개를 반환할 것인지를 인자로
    {
        if(vtlist == null){
            string jdata = File.ReadAllText(Application.dataPath + "/VocaTicket.json");
            vtlist = JsonConvert.DeserializeObject<List<VocaTicket>>(jdata);
            jdata = File.ReadAllText(Application.dataPath + "/VocaTicketMeta.json");
            vtm = JsonConvert.DeserializeObject<VocaTicketMeta>(jdata);
        }

        List<Voca> voca = new List<Voca>();
        int __max = BEGINNER+INTERMEDIATE+ADVANCED+TOEIC;
        
        for(int i=0;i<num;i++){
            int t = Random.Range(0, vtm.sum-1);
            int j;
            
            for(j=0;t>-1;j++){ // 서치
                t-=vtlist[j].sum;
                if(t<0){
                    t+=vtlist[j].sum;
                    break;
                }
            }
            
            int k;
            for(k=0;t>-1;k++){
                t-=vtlist[j].ticket[k];
                if(t<0){
                    break;
                }
            }
            print(j);
            print(k);
            
            if(j<BEGINNER/20){
                voca.Add(beginnerVoca[j*20+k]);
            }
            else if(j<(BEGINNER+INTERMEDIATE)/20){
                voca.Add(intermediateVoca[(j-(BEGINNER/20))*20+k]);
            }
            else if(j<(BEGINNER+INTERMEDIATE+ADVANCED)/20){
                voca.Add(advancedVoca[(j-(BEGINNER+INTERMEDIATE)/20)*20+k]);
            }
            else if(j<(BEGINNER+INTERMEDIATE+ADVANCED)/20){
                voca.Add(toeicVoca[(j-(BEGINNER+INTERMEDIATE+ADVANCED)/20)*20+k]);
            }
        }
        return voca;
    }

    /// <summary>
    /// 단어에 새로운 티켓을 부여함. 
    /// </summary>
    public void SaveVocaTicket(List<Voca> voca, int[] newTicket) // newTicket은 순서대로 새로 부여할티켓
    {
        if(!File.Exists(Application.dataPath + "/VocaTicket.json")){
            InitVocaTicket();
        }

        if(vtlist == null){
            string jdata = File.ReadAllText(Application.dataPath + "/VocaTicket.json");
            vtlist = JsonConvert.DeserializeObject<List<VocaTicket>>(jdata);
            jdata = File.ReadAllText(Application.dataPath + "/VocaTicketMeta.json");
            vtm = JsonConvert.DeserializeObject<VocaTicketMeta>(jdata);
        }
        //불러오기
         if(beginnerVoca == null){
            JsonLoad();
        }
        for(int i=0;i<newTicket.Length;i++){
            int __temp;
            switch(voca[i].difficulty){
                case 0:
                    __temp = vtlist[(int)((voca[i].num-1)/20)].ChangeTicket((voca[i].num-1)%20, newTicket[i]);
                    vtm.AddTicket(__temp);
                    break;
                case 1:
                    __temp = vtlist[(int)((voca[i].num-1+BEGINNER)/20)].ChangeTicket((voca[i].num-1)%20, newTicket[i]);
                    vtm.AddTicket(__temp);
                    break;
                case 2:
                    __temp = vtlist[(int)((voca[i].num-1+BEGINNER+INTERMEDIATE)/20)].ChangeTicket((voca[i].num-1)%20, newTicket[i]);
                    vtm.AddTicket(__temp);
                    break;
                case 3:
                    __temp = vtlist[(int)((voca[i].num-1+BEGINNER+INTERMEDIATE+ADVANCED)/20)].ChangeTicket((voca[i].num-1)%20, newTicket[i]);
                    vtm.AddTicket(__temp);
                    break;
            }
        }
        jdata = JsonConvert.SerializeObject(vtlist);
        File.WriteAllText(Application.dataPath + "/VocaTicket.json", jdata);
        jdata = JsonConvert.SerializeObject(vtm);
        File.WriteAllText(Application.dataPath + "/VocaTicketMeta.json", jdata);
        //저장
    }
}
