using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voca
{
    public int num;
    public string voca;
    public string[] meaning;
    public int difficulty;

    public Voca(int num, string voca, string[] meaning, int difficulty)
    {
        this.num = num;
        this.voca = voca;
        this.meaning = meaning;
        this.difficulty = difficulty;
    }
}
