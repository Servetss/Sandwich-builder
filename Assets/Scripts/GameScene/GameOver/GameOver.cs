using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private const string _recordSave = "Record";


    public static GameOver instance;

    public bool isGameOver { get; private set; }


    [SerializeField] private GameObject _moneyAdd;
    [SerializeField] private Text _moneyText;
    [SerializeField] private GameObject _score;

    private bool _isNewRecord;

    [Space]
    [Header("Final Texts")]
    [SerializeField] private Text _scoreNow;
    [SerializeField] private GameObject _recordCowm;
    [SerializeField] private GameObject _newRecordText;
    [SerializeField] private GameObject _record;
    [SerializeField] private Text _record_T;
    [SerializeField] private Text _ccal;


    [Space]
    [SerializeField] private Animation _cameraAnimation;
    [SerializeField] private GameObject _firewarkFX;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {


        isGameOver = false;
        gameObject.SetActive(false);
    }


    public void GameOverFunk()
    {
        isGameOver = true;
        Time.timeScale = 1;

        GameManager.instance.GameOverFunc();


        Invoke("WaitForStatsCounting", 2);
        _cameraAnimation.Play();

        TextRenderer();
        SetRecord();

        TinySauce.OnGameFinished(Score.instance.GetScore());
    }




    private void TextRenderer()
    {
        print("Active Money: " + Wallet.activeMoney);
        if (Wallet.activeMoney > 0)
        {
            print("Show active Money");
            _moneyAdd.SetActive(true);
            _moneyText.text = "+ " + Wallet.activeMoney.ToString();
        }
        else _moneyAdd.SetActive(false);

        _ccal.text = Burger.instance.GetBurgerKcal() + " Kcal";
    }


    private void SetRecord()
    {
        if (Score.instance.GetScore() > PlayerPrefs.GetInt(_recordSave))
        {
            _isNewRecord = true;
            PlayerPrefs.SetInt(_recordSave, Score.instance.GetScore());
        }

        _scoreNow.text = Score.instance.GetScore().ToString();

        if (_isNewRecord)
        {
            _newRecordText.SetActive(true);
            _recordCowm.SetActive(true);
            _firewarkFX.SetActive(true);
            _firewarkFX.GetComponent<Firewark>().PlayFX();
        }
        else
        {
            _record.SetActive(true);
            _record_T.text = PlayerPrefs.GetInt(_recordSave).ToString();
        }
    }


    private void WaitForStatsCounting()
    {
        gameObject.SetActive(true);
        _score.SetActive(false);

        Wallet.MoveActiveMoneyToPassive();
    }



    public void ResetData()
    {
        isGameOver = false;
        gameObject.SetActive(false);

        _firewarkFX.SetActive(false);

        _isNewRecord = false;
        _newRecordText.SetActive(false);
        _recordCowm.SetActive(false);

        _record.SetActive(false);
    }
}
