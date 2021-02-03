using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllMissionProgressBar : MonoBehaviour
{
    [SerializeField] private Image _progressbarAllMission;
    private float _missionMax;


    public void SetFromStart(int missionMax)
    {
        _missionMax = missionMax;
    }


    public void FillProgressbasByMissionPass(float missionCompleted)
    {
        _progressbarAllMission.fillAmount = missionCompleted / _missionMax;
    }
}
