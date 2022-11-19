using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    public static Mission Instance;
    [SerializeField] private GameObject alarm;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DateTime last = SaveLoad.Instance.GetLastPlayedDateTime();
        DateTime current = DateTime.Now;

        Calendar calendar = new KoreanCalendar();
        int lastWeek = calendar.GetWeekOfYear(last, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        int currentWeek = calendar.GetWeekOfYear(current, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        // x번째 주 계산해서 다르면 초기화
        if (lastWeek < currentWeek)
        {
            Debug.Log("주간 초기화");
            SaveLoad.Instance.ResetWeekStudyMission();
        }

        // 접속일이 다르면 초기화
        if (last.Day != current.Day)
        {
            Debug.Log("일간 초기화");
            SaveLoad.Instance.ResetTodayStudyMission();
        }
    }

    public void ShowAlarm(bool active)
    {
        alarm.SetActive(active);
    }

    public void OnSuccessTodayStudyMission()
    {

    }
    public void GetRewordTodayStudyMission()
    {
        // SaveLoad.Instance.AddTicket(1);
    }
    public void WeekStudyMissionSuccess()
    {
        // SaveLoad.Instance.AddTicket(1);
    }

}
