using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Animator _mainManuButtonAnimator;

    private MainMenu _mainMenu;


    private void Awake()
    {
        _mainMenu = GetComponent<MainMenu>();
    }

    public void StartGame()
    {
        Application.LoadLevel("MainScene");
    }

    public void OpenShop()
    {
        HideMainMenuButton();
        Invoke("WaitForStartOpenShop", 0.5f);   
    }

    private void WaitForStartOpenShop()
    {
        _mainMenu.OpenTheShop();
    }

    private void HideMainMenuButton()
    {
        _mainManuButtonAnimator.Play("ButtonMoveAnim");
    }

    public void ShowMainMenuButton()
    {
        _mainManuButtonAnimator.Play("ButtonMoveToMainMenu");
    }


}
