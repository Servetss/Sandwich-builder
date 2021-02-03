using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMission
{
    int missionLevel { get; set; }

    int maxMissionLevel { get; set; }

    bool IsMissionEnd();

    string GetMissionName();

    float GetMissionProgress();

    bool IsMissionCompleted();

    void IngredientHit(Hit hitType, IngredientSO ingredient);

    void SaveData();

    void MissionCompleted();

    void GameModeFinished();
}
