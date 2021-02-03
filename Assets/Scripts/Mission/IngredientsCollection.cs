using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsCollection : IMission
{
    private MissionManager _missionManager;

    private const string _saveName = "IngredientsCollection";
    private const string _level = "LevelSave";
    private const string _collectNow = "CollectedNow";
    private const string _missionFinished = "MissionFinished";

    public int missionLevel { get; set; }
    public int maxMissionLevel { get; set; }

    private const int _firstLevelPoint = 50;

    private float _ingredientsToMisiionPass;
    private float _ingredientsCollected;

    private bool _isMissionFinished;
    public IngredientsCollection(MissionManager missionManage, int maxMissionLevel)
    {
        this.maxMissionLevel = maxMissionLevel;

        LoadData();
        _missionManager = missionManage;
    }

    private void LoadData()
    {
        missionLevel = PlayerPrefs.GetInt(_saveName + _level) == 0 ? 1 : PlayerPrefs.GetInt(_saveName + _level);


        _ingredientsCollected = PlayerPrefs.GetFloat(_saveName + _collectNow);
        _isMissionFinished = PlayerPrefs.GetInt(_saveName + _missionFinished) == 1 ? true : false;

        _ingredientsToMisiionPass = missionLevel * _firstLevelPoint;

        //Reset();
    }

    private void Reset()
    {
        missionLevel = 1;
        _ingredientsToMisiionPass = missionLevel * _firstLevelPoint;
        _ingredientsCollected = 0;
        _isMissionFinished = false;

        SaveData();
    }


    public string GetMissionName()
    {
        return "Score  " + Mathf.Clamp(_ingredientsCollected, 0, _ingredientsToMisiionPass) + "/" + _ingredientsToMisiionPass;
    }

    public float GetMissionProgress()
    {
        return Mathf.Clamp((_ingredientsCollected / _ingredientsToMisiionPass) * 100, 0, 100);
    }

    public bool IsMissionCompleted()
    {
        return _ingredientsCollected >= _ingredientsToMisiionPass;
    }


    public void IngredientHit(Hit hitType, IngredientSO ingredient)
    {
        if (hitType != Hit.Fall)
        {
            _ingredientsCollected++;
            

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
        _ingredientsToMisiionPass = missionLevel * _firstLevelPoint;
        _ingredientsCollected = 0;
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(_saveName + _level, missionLevel);
        PlayerPrefs.SetFloat(_saveName + _collectNow, _ingredientsCollected);
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
