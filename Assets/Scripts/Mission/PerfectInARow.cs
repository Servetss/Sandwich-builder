using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectInARow : IMission
{
    private MissionManager _missionManager;

    private const string _saveName = "PerfectInARow";
    private const string _level = "LevelSave";
    private const string _collectNow = "CollectedNow";
    private const string _missionFinished = "MissionFinished";

    public int missionLevel { get; set; }
    public int maxMissionLevel { get; set; }

    private const int _firstLevelPoint = 5;

    private float _perfectsToMisiionPass;
    private float _perfectsCollected;

    private bool _isMissionFinished;

    public PerfectInARow(MissionManager missionManage, int maxMissionLevel)
    {
        this.maxMissionLevel = maxMissionLevel;

        LoadData();
        _missionManager = missionManage;
    }

    private void LoadData()
    {
        missionLevel = PlayerPrefs.GetInt(_saveName + _level) == 0 ? 1 : PlayerPrefs.GetInt(_saveName + _level);


        _perfectsCollected = PlayerPrefs.GetFloat(_saveName + _collectNow);
        _isMissionFinished = PlayerPrefs.GetInt(_saveName + _missionFinished) == 1 ? true : false;

        _perfectsToMisiionPass = missionLevel * _firstLevelPoint;

        //Reset();
    }

    private void Reset()
    {
        missionLevel = 1;
        _perfectsToMisiionPass = missionLevel * _firstLevelPoint;
        _perfectsCollected = 0;
        _isMissionFinished = false;

        SaveData();
    }

    public string GetMissionName()
    {
        return "Collect "+ _perfectsToMisiionPass + " perfects in a single game";
    }

    public float GetMissionProgress()
    {
        return Mathf.Clamp((_perfectsCollected / _perfectsToMisiionPass) * 100, 0, 100);
    }

    public void IngredientHit(Hit hitType, IngredientSO ingredient)
    {
        if (!IsMissionCompleted())
        {
            if (hitType == Hit.Perfect) _perfectsCollected++;
            else if (hitType != Hit.Perfect) _perfectsCollected = 0;

            if (_isMissionFinished == false && IsMissionCompleted())
            {
                _isMissionFinished = true;
                _missionManager.MissionFinished(this);
            }
        }
    }

    public bool IsMissionCompleted()
    {
        return _perfectsCollected >= _perfectsToMisiionPass;
    }

    public void MissionCompleted()
    {
        _isMissionFinished = false;
        missionLevel++;
        _perfectsToMisiionPass = missionLevel * _firstLevelPoint;
        _perfectsCollected = 0;
        SaveData();
    }



    public void SaveData()
    {
        PlayerPrefs.SetInt(_saveName + _level, missionLevel);
        PlayerPrefs.SetFloat(_saveName + _collectNow, _perfectsCollected);
        PlayerPrefs.SetInt(_saveName + _missionFinished, _isMissionFinished ? 1 : 0);
    }

    public void GameModeFinished()
    {
        if (!IsMissionCompleted())
        {
            _isMissionFinished = false;
            _perfectsCollected = 0;
        }
        SaveData();
    }

    public bool IsMissionEnd()
    {
        return maxMissionLevel <= missionLevel;
    }
}
