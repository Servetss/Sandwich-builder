using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // ~~~ Save ~~
    private const string _saveName = "Timer";
    [SerializeField] private string _timerName;
    private const string _timerDuartionIndexSave = "Duration";
    private const string _timerInSecondSave = "TimerInSecond";
    private const string _secondsSave = "Seconds";
    private const string _minutesSave = "Minute";
    private const string _hoursSave = "Hour";


    [Header("UI")]
    [SerializeField] private GameObject _blockPanel;
    [SerializeField] private GameObject _missionPanel;
    [SerializeField] private Text _seconds_T;
    [SerializeField] private Text _minute_T;
    [SerializeField] private Text _houre_T;


    // ~~ Timer durations
    private int[] _timerDurationsInMinutes = { 2, 4, 6 };
    private int _timerDurationIndex;

    // ~~ Time parametrs ~~
    private int _timerEndInSeconds;
    private int _seconds;
    private int _minutes;
    private int _hours;


    private void Awake()
    {
        LoadTimer();
    }


    public void TimerStart(int second)
    {
        _missionPanel.SetActive(false);
        _blockPanel.SetActive(true);
        SetStartTimeParametrsBySeconds(second);
        InvokeRepeating("AddSecond", 1, 1);
    }

    public void TimerStart()
    {
        _blockPanel.SetActive(true);
        _missionPanel.SetActive(false);
        SetStartTimeParametrsBySeconds(_timerDurationsInMinutes[_timerDurationIndex] * 60);
        InvokeRepeating("AddSecond", 1, 1);
    }


    private void SetStartTimeParametrsBySeconds(int seconds)
    {
        _timerEndInSeconds = seconds;

        _hours = Mathf.FloorToInt(seconds / 60 / 60);
        seconds -= (_hours * 60 * 60);

        _minutes = Mathf.FloorToInt(seconds / 60);
        seconds -= (_minutes * 60);

        _seconds = seconds;

        RenderTimerText();
    }



    private void AddSecond()
    {
        _seconds--;
        _timerEndInSeconds--;

        SetTimeParametrs();
        RenderTimerText();

        if (_timerEndInSeconds <= 0) TimerFinished();
    }

    public int GetTimeToTimerFinish()
    {
        return _timerEndInSeconds;
    }

    public bool GetIsTimerActive()
    {
        return _timerEndInSeconds > 0;
    }


    private void SetTimeParametrs()
    {
        if (_minutes > 0 && _seconds < 0)
        {
            _seconds = 59;
            _minutes--;
        }
        if (_hours > 0 && _minutes < 0)
        {
            _minutes = 59;
            _hours--;
        }
    }

    private void RenderTimerText()
    {
        _seconds_T.text = PrepareNumberText(_seconds);
        _minute_T.text = PrepareNumberText(_minutes);
        _houre_T.text = PrepareNumberText(_hours);
    }


    private string PrepareNumberText(int number)
    {
        return number < 10 ? "0" + number : number.ToString();
    }



    public void TimerFinished()
    {
        CancelInvoke("AddSecond");

        _timerEndInSeconds = 0;
        ChangeTimerDuration();
        SaveTimer();
        _blockPanel.SetActive(false);
        _missionPanel.SetActive(true);
    }

    public void ResetTimer()
    {
        _timerEndInSeconds = 0;
        SaveTimer();
        _blockPanel.SetActive(false);
        _missionPanel.SetActive(true);
    }


    private void ChangeTimerDuration()
    {
        _timerDurationIndex++;
        if (_timerDurationIndex >= _timerDurationsInMinutes.Length)
        {
            _timerDurationIndex = 0;
        }
    }



    private void OnApplicationQuit()
    {
        SaveTimer();
    }

    public void SaveTimer()
    {
        PlayerPrefs.SetInt(_saveName + _timerName + _timerInSecondSave, _timerEndInSeconds);
        PlayerPrefs.SetInt(_saveName + _timerName + _timerDuartionIndexSave, _timerDurationIndex);
    }


    private void LoadTimer()
    {
        _timerEndInSeconds = PlayerPrefs.GetInt(_saveName + _timerName + _timerInSecondSave);
        _timerDurationIndex = PlayerPrefs.GetInt(_saveName + _timerName + _timerDuartionIndexSave);
        
        RenderTimerText();
    }
}
