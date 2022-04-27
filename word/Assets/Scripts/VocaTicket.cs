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
    
    public int ChangeTicket(int num, int t){ // 티켓 총 변화량 반환
        int temp = t-ticket[num];
        sum += temp;
        ticket[num] = t;
        return temp;
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

    public void AddTicket(int t){
        sum+=t;
    }

}
