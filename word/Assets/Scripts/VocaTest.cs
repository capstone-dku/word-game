using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

class word
{
    public int num;
    public string voca;
    public string[] mean;
    public int grade;

    public word(int num, string voca, string[] mean, int grade)
    {
        this.num = num;
        this.voca = voca;
        this.mean = mean;
        this.grade = grade;
    }
}

public class VocaTest : MonoBehaviour
{
    List<word> data = new List<word>();
    VocaSelector vs = new VocaSelector();
    private void Start()
    {
        List<Voca> voca = vs.SelectVoca(3,1);
        print(voca[3].meaning[0]);
    }

    public void json_save()
    {
        string jdata = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath + "/word_database.json", jdata);
    }

    public void json_load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/word_database.json");
        print(jdata);
        data = JsonConvert.DeserializeObject<List<word>>(jdata);
        print(data[0].mean);
    }
}
