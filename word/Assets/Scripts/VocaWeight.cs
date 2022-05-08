using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocaWeight
{
    public int[] weight;
    public int sum;
    public VocaWeight(int t)
    {
        weight = new int[20];
        for(int i=0;i<20;i++)
        {
            weight[i] = t;
        }
        sum = t*20;
            
    }
    
    public int ChangeWeight(int num, int t){ // 가중치 총 변화량 반환
        int temp = t-weight[num];
        sum += temp;
        weight[num] = t;
        return temp;
    }
}

public class VocaWeightMeta
{
    public int[] offset;
    public int sum;
    public VocaWeightMeta(int b, int i, int a, int t)
    {
        offset = new int[4]{b, i , a, t};
        sum = 0;
    }

    public void AddWeight(int t){
        sum+=t;
    }

}
