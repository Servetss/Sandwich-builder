using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectsCollection : IMission
{
    private MissionManager _missionManager;

    private const string _saveName = "PerfectsCollection";
    private const string _level = "LevelSave";
    private const string _collectNow = "CollectedNow";
    private const string _missionFinished = "MissionFinished";


    public int missionLevel { get; set; }
    public int maxMissionLevel { get; set; }

    private const int _firstLevelPoint = 15;

    private float _perfectsToMisiionPass;
    private float _perfectsCollected;

    private bool _isMissionFinished;


    // Mission End
    private bool _isMissionBranchEnd;


    public PerfectsCollection(MissionManager missionManage, int maxMissionLevel)
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
        return "Collect perfects " + Mathf.Clamp(_perfectsCollected, 0, _perfectsToMisiionPass) + "/" +_perfectsToMisiionPass;
    }


    public float GetMissionProgress()
    {
        return Mathf.Clamp((_perfectsCollected / _perfectsToMisiionPass) * 100, 0, 100);
    }


    public bool IsMissionCompleted()
    {
        return _perfectsCollected >= _perfectsToMisiionPass;
    }


    public void IngredientHit(Hit hitType, IngredientSO ingredient)
    {
        if (hitType == Hit.Perfect)
        {
            _perfectsCollected++;
            

            if (_isMissionFinished == false && IsMissionCompleted())
            {
                _isMissionFinished = true;
                _missionManager.MissionFinished(this);
            }
        }
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
        SaveData();
    }

    public bool IsMissionEnd()
    {
        return maxMissionLevel <= missionLevel;
    }
}
