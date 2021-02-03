using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    private const string _firstMissionName = "FirstMission";
    private const string _secondMissionName = "SecondMission";

    private const int _moneyForCompletedmission = 20;

    // ~~ MISSIONS RENDERER PANEL
    [SerializeField] private MissionRenderer _rendererMissionOnce;
    [SerializeField] private MissionRenderer _rendererMissionWhole;


    // ~~ ALL MISSION ~~
    private IMission[] _firstMissions;
    private IMission[] _secondMissions;


    // ~~ MISSIONS NOW ~~
    private IMission _firstMission;
    private IMission _secondMission;


    private int _firstSelectedMissionNum;
    private int _secondSelectedMissionNum;

    
    // ~~ Mission Finish Text
    [Space]
    [SerializeField] private Animation _missionFinishAnimation;
    [SerializeField] private Text _missionFinishText;
    [SerializeField] private AllMissionProgressBar _allMissionProgressbar;


    private void Awake()
    {
        int maxMissionLevel = 8; 

        _firstMissions = new IMission[2] { new PerfectsCollection(this, maxMissionLevel), new IngredientsCollection(this, maxMissionLevel) };
        _secondMissions = new IMission[2] { new PerfectInARow(this, maxMissionLevel), new IngredientInARow(this, maxMissionLevel) };
    }

    private void Start()
    {
        MissionLoad();
    }



    private void MissionLoad()
    {
        _firstSelectedMissionNum = PlayerPrefs.GetInt(_firstMissionName);
        _secondSelectedMissionNum = PlayerPrefs.GetInt(_secondMissionName);

        _allMissionProgressbar.SetFromStart(GetMaxMissionCanBeCompleted());

        _firstMission = GetNextUnlockMissionOrNull(ref _firstSelectedMissionNum, _firstMissions); 
        _secondMission = GetNextUnlockMissionOrNull(ref _secondSelectedMissionNum , _secondMissions);

        MissionButtonsFromStart();

        _rendererMissionOnce.SetNewMission(_firstMission, _firstMission.missionLevel);
        _rendererMissionWhole.SetNewMission(_secondMission, _secondMission.missionLevel);

        SetFirstMissionsRenderer();
        SetSecondMissionsRenderer();

        _allMissionProgressbar.FillProgressbasByMissionPass(GetAllCompletedMission());
    }

    private void MissionButtonsFromStart()
    {
        if (CheckIsPlayerHaveOneMissionTypeInBranchToEnding(_firstMissions)) _rendererMissionOnce.SetWhenPlayerHaveOneMissionTypeInBranch();
        if (CheckIsAllMissionBranchEnd(_firstMissions)) _rendererMissionOnce.SetWhenMissionBranchHaveEnded();
        


        if (CheckIsPlayerHaveOneMissionTypeInBranchToEnding(_secondMissions)) _rendererMissionWhole.SetWhenPlayerHaveOneMissionTypeInBranch();
        if (CheckIsAllMissionBranchEnd(_secondMissions)) _rendererMissionWhole.SetWhenMissionBranchHaveEnded();    
    }


    // Click on button
    public void CompleteFirstMission()
    {
        SetFirstMissions();
        GameTime.instance.SaveTime();
    }

    public void CompleteSecondMissionPass()
    {
        SetSecondMission();
        GameTime.instance.SaveTime();
    }
    //



    public void MissionDo(Hit hitType, IngredientSO ingredient)
    {
        if (_rendererMissionOnce.GetIsTImerActive() == false) _firstMission.IngredientHit(hitType, ingredient);
        if (_rendererMissionWhole.GetIsTImerActive() == false) _secondMission.IngredientHit(hitType, ingredient);
    }


    public void MissionFinished(IMission mission)
    {
        _missionFinishText.text = mission.GetMissionName();
        _missionFinishAnimation.Play();
    }

    public void EndGame()
    {
        _firstMission.GameModeFinished();
        _secondMission.GameModeFinished();
        
        SetFirstMissionsRenderer();
        SetSecondMissionsRenderer();

        GameTime.instance.SaveTime();
    }

    #region Missions renderer on panels
    private void SetFirstMissionsRenderer()
    {
        _rendererMissionOnce.ChangeProgress(_firstMission);
        _rendererMissionOnce.SetButtonNextMissionActive(_firstMission.IsMissionCompleted());
    }

    private void SetSecondMissionsRenderer()
    {
        _rendererMissionWhole.ChangeProgress(_secondMission);
        _rendererMissionWhole.SetButtonNextMissionActive(_secondMission.IsMissionCompleted());
    }
    #endregion


    #region Change missions
    public void ChangeFirstMission()
    {
        _firstMission.MissionCompleted();
        SetFirstMissions();
        _rendererMissionOnce.SetButtonNextMissionActive(false);
        _rendererMissionOnce.SetTimer();

        Wallet.AddPassiveMoney(_moneyForCompletedmission);
        _allMissionProgressbar.FillProgressbasByMissionPass(GetAllCompletedMission());
        GameTime.instance.SaveTime();
    }

    public void ChangeSecondMission()
    {
        _secondMission.MissionCompleted();
        SetSecondMission();
        _rendererMissionWhole.SetButtonNextMissionActive(false);
        _rendererMissionWhole.SetTimer();

        Wallet.AddPassiveMoney(_moneyForCompletedmission);
        _allMissionProgressbar.FillProgressbasByMissionPass(GetAllCompletedMission());
        GameTime.instance.SaveTime();
    }
    #endregion



    private void SetFirstMissions()
    {
        _firstMission = GetNextUnlockMissionOrNull(ref _firstSelectedMissionNum, _firstMissions);

        _rendererMissionOnce.SetNewMission(_firstMission, _firstMission.missionLevel);
        SetFirstMissionsRenderer();
    }

    private void SetSecondMission()
    {
        _secondMission = GetNextUnlockMissionOrNull(ref _secondSelectedMissionNum, _secondMissions);
        _rendererMissionWhole.SetNewMission(_secondMission, _secondMission.missionLevel);
        SetSecondMissionsRenderer();
    }

    private IMission GetNextUnlockMissionOrNull(ref int indexMissionNow, IMission[] missionList)
    {
        for (int i = 0; i < missionList.Length; i++)
        {
            if (indexMissionNow + 1 < missionList.Length)
                indexMissionNow++;
            else indexMissionNow = 0;
            

            if (missionList[indexMissionNow].IsMissionEnd() == false)
            {
                return missionList[indexMissionNow];
            }
        }


        return new EndMissionForm();
    }

    private bool CheckIsPlayerHaveOneMissionTypeInBranchToEnding(IMission[] missionList)
    {
        int missionNotEnded = 0;
        for (int i = 0; i < missionList.Length; i++)
        {
            if (missionList[i].IsMissionEnd() == false) missionNotEnded++;
        }

        return missionNotEnded == 1;
    }

    private bool CheckIsAllMissionBranchEnd(IMission[] missionList)
    {
        for (int i = 0; i < missionList.Length; i++)
        {
            if (missionList[i].IsMissionEnd() == false) return false;
        }

        return true;
    }

    private int GetMaxMissionCanBeCompleted()
    {
        int maxMissionCompleted = 0;

        for (int i = 0; i < _firstMissions.Length; i++)
        {
            maxMissionCompleted += (_firstMissions[i].maxMissionLevel - 1);
        }

        for (int i = 0; i < _secondMissions.Length; i++)
        {
            maxMissionCompleted += (_secondMissions[i].maxMissionLevel - 1);
        }

        return maxMissionCompleted;
    }

    private int GetAllCompletedMission()
    {
        int allMissionCompeleted = 0;

        for (int i = 0; i < _firstMissions.Length; i++)
        {
            allMissionCompeleted += (_firstMissions[i].missionLevel - 1);
        }

        for (int i = 0; i < _secondMissions.Length; i++)
        {
            allMissionCompeleted += (_secondMissions[i].missionLevel - 1);
        }

        return allMissionCompeleted;
    }


    private void OnDisable()
    {
        MissionSave();
    }

    private void MissionSave()
    {
        PlayerPrefs.SetInt(_firstMissionName, _firstSelectedMissionNum);
        PlayerPrefs.SetInt(_secondMissionName, _secondSelectedMissionNum);
    }
}
