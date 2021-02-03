using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;



public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [Space]
    [Header("Canvas")]
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _gameOverCanvas;

    [Space]
    [Header("Main menu panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _missionPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _tutorialPanel;

    private PlayerControl _playerController;
    private ThrowDirection _throwDirection;

    [Space]
    [Header("Game over buttons")]
    [SerializeField] private GameObject _mainMenuButton;


    public bool isSettingsPanelOpen { get; private set; }
    private bool isGameOverCanvasObjectWasActive;

    private const string _leaderBoard = "CgkI-JG4tacDEAIQAQ";

    private void Awake()
    {
        _playerController = _player.GetComponent<PlayerControl>();
        _throwDirection = _player.GetComponent<ThrowDirection>();

        _shopPanel.SetActive(true); // Set off from mission panel
        _missionPanel.SetActive(true); // Set off from mission panel
        _settingsPanel.SetActive(true); // Set off from mission panel

    }

    private void Start()
    {
        //PlayerPrefs.SetInt("Record", 0);

        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => Authentication(success, "Authenticate: "));
        

        _missionPanel.SetActive(false);
    }

    private void Authentication(bool isSuccess, string actionName)
    {
        if (isSuccess) print(actionName + "ПОБЕДА !");
        else print(actionName + "Беда !");
    }

    public void StartGame()
    {
        _playerController.enabled = true;
        _throwDirection.enabled = true;
        GameManager.instance.StartGame();
        DisabledMainMenuCanvas();
    }


    public void EnabledMainMenuCanvas()
    {
        _mainMenuCanvas.SetActive(true);
        _gameCanvas.SetActive(false);
    }

    public void DisabledMainMenuCanvas()
    {
        _mainMenuCanvas.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    public void OpenShop()
    {
        if (CanOpenPanel())
        {
            isGameOverCanvasObjectWasActive = _gameOverCanvas.activeSelf;

            _gameOverCanvas.SetActive(false);
            _mainMenuPanel.SetActive(false);
            _shopPanel.SetActive(true);
        }
    }

    public void CloseShop()
    {
        _gameOverCanvas.SetActive(isGameOverCanvasObjectWasActive);
        _mainMenuPanel.SetActive(true);
        _shopPanel.SetActive(false);
    }

    public void OpenMission()
    {
        if (CanOpenPanel())
        {
            isGameOverCanvasObjectWasActive = _gameOverCanvas.activeSelf;

            _gameOverCanvas.SetActive(false);
            _mainMenuPanel.SetActive(false);
            _missionPanel.SetActive(true);

            GameTime.instance.SaveTime();
        }
    }

    public void CloseMission()
    {
        _gameOverCanvas.SetActive(isGameOverCanvasObjectWasActive);
        _mainMenuPanel.SetActive(true);
        _missionPanel.SetActive(false);
    }


    public void OpenSettings()
    {
        isSettingsPanelOpen = true;
        _settingsPanel.SetActive(true);
        _playButton.SetActive(false);

        _mainMenuButton.SetActive(false);
    }

    public void CloseSettings()
    {
        isSettingsPanelOpen = false;
        _settingsPanel.SetActive(false);
        _playButton.SetActive(true);


        _mainMenuButton.SetActive(true);
    }


    public void OpenTutorial()
    {
        _mainMenuPanel.SetActive(false);
        _tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        _mainMenuPanel.SetActive(true);
        _tutorialPanel.SetActive(false);
    }

    private bool CanOpenPanel()
    {
        return isSettingsPanelOpen == false;
    }

    public void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }


    public void SetNewHeight(int height)
    {
        Social.ReportScore(height, _leaderBoard, (bool success) => Authentication(success, "Set new height: "));
    }
}
