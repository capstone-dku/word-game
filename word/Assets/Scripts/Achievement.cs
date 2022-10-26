using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DateTime last = SaveLoad.Instance.GetLastPlayedDateTime();
        DateTime current = DateTime.Now;
        if (last.Month != current.Month)
        {
            // 해당 월에 처음으로 로그인
        }

        if (last.Month == current.Month && last.Day != current.Day)
        {
            // 해당 일에 처음으로 로그인
        }
        if (last < current && current.DayOfWeek == DayOfWeek.Sunday)
        {
            
        }
        Debug.Log(last.DayOfYear);
        Debug.Log(last);
        Debug.Log(current);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
