using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocaTicket
{
    public int[] ticket;
    public int sum;
    public VocaTicket(int t)
    {
        ticket = new int[20];
        for(int i=0;i<20;i++)
        {
            ticket[i] = t;
        }
        sum = t*20;
            
    }
    
    public void ChangeTicket(int num, int t){
        sum += t - ticket[num];
        ticket[num] = 10;
    }
}

public class VocaTicketMeta
{
    public int[] offset;
    public int sum;
    public VocaTicketMeta(int b, int i, int a, int t)
    {
        offset = new int[4]{b, i , a, t};
        sum = 0;
    }
}
