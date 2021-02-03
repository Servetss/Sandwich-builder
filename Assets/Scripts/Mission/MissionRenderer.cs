using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionRenderer : MonoBehaviour
{
    [SerializeField] private Image _Image;
    [SerializeField] private Text _missionExplain;

    [Space]
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private Image _progressImage;
    [SerializeField] private Text _progressText;

    [Space]
    [SerializeField] private Text _levelText;

    [Space]
    [SerializeField] private GameObject _buttonNextMission;
    [SerializeField] private GameObject _buttonMissionPass;
    [SerializeField] private Timer _timer;


    [Space]
    [SerializeField] private GameObject _missionPanel;
    [SerializeField] private GameObject _timerPanel;


    private bool _isLastMissionTypeInBranch;
    private bool _isMissionsInBranchHaveEnded;

    public void SetNewMission(IMission mission, int level)
    {
        _missionExplain.text = mission.GetMissionName();
        _levelText.text = level.ToString();


        ChangeProgress(mission);
    }



    public void ChangeProgress(IMission mission)
    {
        _progressImage.fillAmount = mission.GetMissionProgress() / 100;
        _progressText.text = Mathf.Floor(mission.GetMissionProgress()) + "%";

        _missionExplain.text = mission.GetMissionName();
    }


    public void SetButtonNextMissionActive(bool isActive)
    {
        if (_isMissionsInBranchHaveEnded == false)
        {
            _buttonNextMission.SetActive(isActive);
            _buttonMissionPass.SetActive(!isActive && !_isLastMissionTypeInBranch);
        }
    }

    public void SetTimer()
    {
        _timer.TimerStart();
    }

    public bool GetIsTImerActive()
    {
        return _timer.GetIsTimerActive();
    }



    public void SetWhenPlayerHaveOneMissionTypeInBranch()
    {
        _buttonMissionPass.SetActive(false);
        _isLastMissionTypeInBranch = true;
    }


    public void SetWhenMissionBranchHaveEnded()
    {
        _isMissionsInBranchHaveEnded = true;

        _missionPanel.SetActive(false);
        _timerPanel.SetActive(false);
        _progressBar.SetActive(false);

        _buttonNextMission.SetActive(false);
        _buttonMissionPass.SetActive(false);

        _progressImage.gameObject.SetActive(false);
        _progressText.gameObject.SetActive(false);
    }
}
