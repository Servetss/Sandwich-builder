using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public static GameTime instance;

    private const string _format = "dd/MM/yyyy HH:mm:ss";

    public int timePlayerAbsence { get; private set; }


    [Space]
    [Header("Mission timer")]
    [SerializeField] private Timer _firstMissionTimer;
    [SerializeField] private Timer _secondMissionTimer;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timePlayerAbsence = 0;
        StartCoroutine(CheckPlayerAbsenceOnline());
    }


    private void CheckOffLine()
    {
        TimeSpan timeSpan;
        if (PlayerPrefs.HasKey("LastSession"))
        {
            print(DateTime.UtcNow.ToString());
            timeSpan = DateTime.UtcNow - DateTime.Parse(PlayerPrefs.GetString("LastSession"));


            timePlayerAbsence = GetTimeInSeconds(timeSpan);

            
            SetTimers();
            print(string.Format(timeSpan.Days  + " " + timeSpan.Hours + " " + timeSpan.Minutes + " " + timeSpan.Seconds));
            print(timePlayerAbsence);
        }


        SaveOffline();
    }



    private IEnumerator CheckPlayerAbsenceOnline()
    {
        WWW www = new WWW("https://artmind-games.com/TimeParser.php");
        yield return www;

        if (www.error != null)
        {
            print("Ошибка: " + www.error);
            CheckOffLine();
            SaveTime();
            yield break;
        }

        string time = www.text;

        TimeSpan timeSpan;
        if (PlayerPrefs.HasKey("LastSession"))
        {
            timeSpan = DateTime.ParseExact(time, _format, CultureInfo.InvariantCulture) - DateTime.ParseExact(PlayerPrefs.GetString("LastSession"), _format, CultureInfo.InvariantCulture);


            timePlayerAbsence = GetTimeInSeconds(timeSpan);
            print(timePlayerAbsence);
            
            SetTimers();
        }

        SaveTime();
    }


    private void SetTimers()
    {
        int newFirstTimer = _firstMissionTimer.GetTimeToTimerFinish() - timePlayerAbsence;
        int newSecondTimer = _secondMissionTimer.GetTimeToTimerFinish() - timePlayerAbsence;

        if (newFirstTimer > 0)
        {
            _firstMissionTimer.TimerStart(_firstMissionTimer.GetTimeToTimerFinish() - timePlayerAbsence);
        }
        else if (_firstMissionTimer.GetTimeToTimerFinish() > 0)
        {
            _firstMissionTimer.TimerFinished();
        }
        else
        {
            _firstMissionTimer.ResetTimer();
        }

        if (newSecondTimer > 0)
        {
            _secondMissionTimer.TimerStart(_secondMissionTimer.GetTimeToTimerFinish() - timePlayerAbsence);
        }
        else if (_secondMissionTimer.GetTimeToTimerFinish() > 0)
        {
            _secondMissionTimer.TimerFinished();
        }
        else
        {
            _secondMissionTimer.ResetTimer();
        }
    }

    private int GetTimeInSeconds(TimeSpan timeSpan)
    {
        int timeInSeconds = 0;
        timeInSeconds = timeSpan.Days * 24 * 60 * 60;
        timeInSeconds += timeSpan.Hours * 60 * 60;
        timeInSeconds += timeSpan.Minutes * 60;
        timeInSeconds += timeSpan.Seconds;

        return timeInSeconds;
    }


    private void OnApplicationQuit()
    {
        SaveTime();
    }


    public void SaveTime()
    {
        _firstMissionTimer.SaveTimer();
        _secondMissionTimer.SaveTimer();

        StartCoroutine(SaveTimeFromServer());
    }


    private IEnumerator SaveTimeFromServer()
    {
        SaveOffline();

        WWW www = new WWW("https://artmind-games.com/TimeParser.php");
        yield return www;

        if (www.error != null)
        {
            print("Ошибка: " + www.error);
            yield break;
        }

        PlayerPrefs.SetString("LastSession", www.text);
        print("SAVE TIME Online!  " + www.text);
    }

    private void SaveOffline()
    {
        PlayerPrefs.SetString("LastSession", DateTime.UtcNow.ToString());
        print("SAVE TIME Offline! " + DateTime.UtcNow.ToString());
    }
}
