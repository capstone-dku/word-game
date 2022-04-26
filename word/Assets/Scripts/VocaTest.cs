using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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
    word w1 = new word(1,"apple", new string[]{"사과"}, 0);

    private void Start()
    {
        string jdata = JsonConvert.SerializeObject(w1);
        print(jdata);
    }
}
