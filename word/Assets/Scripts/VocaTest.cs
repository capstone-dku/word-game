using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

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
    Stopwatch watch = new Stopwatch();
    private void Start()
    {
        List<Voca> voca;
        int[] t = new int[5]{5,5,5,5,5};
        watch.Start();

        vs.JsonLoad();
        vs.AddVocaWeight(1,0);
        voca = vs.FindVocaWeight(5);
        vs.SaveVocaWeight(voca,t);

        watch.Stop();
        UnityEngine.Debug.Log(watch.ElapsedMilliseconds+" ms");

        for(int i=0;i<voca.Count;i++){
            print(voca[i].voca);
        }
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
