using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMissionForm : IMission
{
    public int missionLevel { get; set; }
    public int maxMissionLevel { get; set; }

    public void GameModeFinished()
    {
        
    }

    public string GetMissionName()
    {
        return "Mission branch was finished";
    }

    public float GetMissionProgress()
    {
        return 0;
    }

    public void IngredientHit(Hit hitType, IngredientSO ingredient)
    {
        
    }

    public bool IsMissionCompleted()
    {
        return false;
    }

    public bool IsMissionEnd()
    {
        return false;
    }

    public void MissionCompleted()
    {

    }

    public void SaveData()
    {
        
    }
}
