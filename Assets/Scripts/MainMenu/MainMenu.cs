using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private enum Scene { MainMenu, Shop }
    private Scene _selectedScene;

    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _shopPanel;

    private MainMenuButton _mainMenuButton;
    private Animator _animatorComp;

    private bool _isOpenTheShop;
    private bool _isOpenTheMainMenu;

    private void Awake()
    {
        _animatorComp = GetComponent<Animator>();
        _mainMenuButton = GetComponent<MainMenuButton>();
    }

    public void OpenTheShop()
    {
        _selectedScene = Scene.Shop;
        _mainMenuPanel.SetActive(false);
        _animatorComp.Play("CameraRotateToShop");
    }

    public void CloseTheShop()
    {
        _selectedScene = Scene.MainMenu;
        _shopPanel.SetActive(false);
        _animatorComp.Play("CameraRotateToMainMenuFromShop");
    }

    public void AnimationsEnd()
    {
        SetPanelActive();

        if (_selectedScene == Scene.MainMenu)
        {
            _mainMenuPanel.SetActive(true);
            _mainMenuButton.ShowMainMenuButton();
        }
    }

    private void SetPanelActive()
    {
        //_mainMenuPanel.SetActive(_selectedScene == Scene.MainMenu);
        _shopPanel.SetActive(_selectedScene == Scene.Shop);
    }
}
