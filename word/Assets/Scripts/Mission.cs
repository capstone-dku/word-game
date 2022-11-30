using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    public static Mission Instance;
    [SerializeField] private GameObject alarm;
    [SerializeField] private Text TodayMissionText;
    [SerializeField] private Button TodayMissionButton;

    [SerializeField] private Button[] WeekMissionButton;
    [SerializeField] private Text[] weekMissionText;
    private bool todayMissionReward = false;
    private bool weekMissionReward = false;
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

    void OnEnable()
    {
        UpdateButton();
    }
    public void UpdateButton()
    {
        int todayMissionCount = SaveLoad.Instance.GetTodayStudyMissionCount();
        bool todayMissionClear = SaveLoad.Instance.GetTodayStudyMissionClear();
        TodayMissionButton.interactable = false;
        if (todayMissionCount >= 1 && !todayMissionClear)
        {
            TodayMissionText.text = todayMissionCount + "/1";
            ShowAlarm(true);
            todayMissionReward = true;
            TodayMissionButton.interactable = true;
        }


        int weekMissionCount = SaveLoad.Instance.GetWeekStudyMissionCount();
        bool weekMissionClear = SaveLoad.Instance.GetWeekStudyMissionClear();
        weekMissionText[0].text = weekMissionCount + "/5";
        weekMissionText[1].text = weekMissionCount + "/10";
        weekMissionText[2].text = weekMissionCount + "/15";

        WeekMissionButton[0].interactable = false;
        WeekMissionButton[1].interactable = false;
        WeekMissionButton[2].interactable = false;
        if (weekMissionCount >= 5 && !weekMissionClear)
        {
            ShowAlarm(true);
            weekMissionReward = true;
            WeekMissionButton[0].interactable = true;
        } 
        if (weekMissionCount >= 10)
        {
            ShowAlarm(true);
            WeekMissionButton[1].interactable = true;
        }

        if (weekMissionCount >= 15)
        {

            ShowAlarm(true);
            WeekMissionButton[2].interactable = true;
        }
    }
    public void OnClickedMission(bool active)
    {
        gameObject.SetActive(active);
    }
    public void ShowAlarm(bool active)
    {
        alarm.SetActive(active);
    }

    public void OnSuccessTodayStudyMission()
    {
        UpdateButton();
    }
    public void GetRewordTodayStudyMission()
    {
        SaveLoad.Instance.AddTicket(1);
        ShowAlarm(false);
    }
    public void GetRewordWeekStudyMission()
    {
        SaveLoad.Instance.AddTicket(1);
        ShowAlarm(false);
    }

}
