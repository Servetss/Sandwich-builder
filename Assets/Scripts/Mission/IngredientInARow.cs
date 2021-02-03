using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInARow : IMission
{
    private MissionManager _missionManager;

    private const string _saveName = "IngredientInARow";
    private const string _level = "LevelSave";
    private const string _collectNow = "CollectedNow";
    private const string _ingredientToSave = "IngredientName";
    private const string _missionFinished = "MissionFinished";

    public int missionLevel { get; set; }
    public int maxMissionLevel { get; set; }

    private const int _firstLevelPoint = 5;

    private float _ingredientsToMisiionPass;
    private float _ingredientsCollected;

    private string _ingredientNameToCollect;

    private bool _isMissionFinished;
    public IngredientInARow(MissionManager missionManager, int maxMissionLevel)
    {
        this.maxMissionLevel = maxMissionLevel;

        LoadData();
        _missionManager = missionManager;
    }

    private void LoadData()
    {
        missionLevel = PlayerPrefs.GetInt(_saveName + _level) == 0 ? 1 : PlayerPrefs.GetInt(_saveName + _level);
        _ingredientNameToCollect = PlayerPrefs.GetString(_saveName + _ingredientToSave) == "" ? "Bread" : PlayerPrefs.GetString(_saveName + _ingredientToSave);

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
        return "Collect " + _ingredientsToMisiionPass + " " + _ingredientNameToCollect + " in a single game";
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
        if (hitType != Hit.Fall && _ingredientNameToCollect.Equals(ingredient.name))
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
        _ingredientNameToCollect = GameManager.instance.GetRandomUnlockIngredient().name;

        _ingredientsCollected = 0;
        
        SaveData();
    }




    public void SaveData()
    {
        PlayerPrefs.SetInt(_saveName + _level, missionLevel);
        PlayerPrefs.SetFloat(_saveName + _collectNow, _ingredientsCollected);
        PlayerPrefs.SetString(_saveName + _ingredientToSave, _ingredientNameToCollect);
        PlayerPrefs.SetInt(_saveName + _missionFinished, _isMissionFinished ? 1 : 0);
    }

    public void GameModeFinished()
    {
        if (!IsMissionCompleted())
        {
            _isMissionFinished = false;
            _ingredientsCollected = 0;
        }

        SaveData();
    }

    public bool IsMissionEnd()
    {
        return maxMissionLevel <= missionLevel;
    }
}
