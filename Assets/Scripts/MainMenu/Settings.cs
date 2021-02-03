using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private const string _musicStatusSave = "MusicSave";
    private const string _vibrationStatusSave = "VibrationSave";


    public static Settings instance;


    // ~~ Status ~~
    public bool isMusicActive { get; private set; }
    public bool isVibrationActive { get; private set; }



    [SerializeField] private Image _musicImage;
    [SerializeField] private Image _vibrationImage;

    [Space]
    [Header("Icons")]
    [SerializeField] private Sprite _musicOnSprite;
    [SerializeField] private Sprite _musicOffSprite;
    [Space]
    [SerializeField] private Sprite _vibrationOnSprite;
    [SerializeField] private Sprite _vibrationOffSprite;
    // Exception for vibration ico (need to change Height param)
    private const float _vibrationOnIcoHeight = 132;
    private const float _vibrationOffIcoHeight = 76;


    private void Awake()
    {
        isMusicActive = PlayerPrefs.GetInt(_musicStatusSave) == 1;
        isVibrationActive = PlayerPrefs.GetInt(_vibrationStatusSave) == 1;

        _musicImage.sprite = isMusicActive ? _musicOnSprite : _musicOffSprite;
        _vibrationImage.sprite = isVibrationActive ? _vibrationOnSprite : _vibrationOffSprite;
        ChangeVibrationRectHeight();


        instance = this;
    }

    private void Start()
    {
        SoundManager.instance.SetAudioSorceMute(isMusicActive);
        gameObject.SetActive(false);
    }

    public void ChangeMusicStatus()
    {
        isMusicActive = !isMusicActive;

        _musicImage.sprite = isMusicActive ? _musicOnSprite : _musicOffSprite;

        PlayerPrefs.SetInt(_musicStatusSave, isMusicActive ? 1 : 0);
        SoundManager.instance.SetAudioSorceMute(isMusicActive);
    }

    public void ChangeVibrationStatus()
    {
        isVibrationActive = !isVibrationActive;

        _vibrationImage.sprite = isVibrationActive ? _vibrationOnSprite : _vibrationOffSprite;

        ChangeVibrationRectHeight();

        PlayerPrefs.SetInt(_vibrationStatusSave, isVibrationActive ? 1 : 0);
    }


    private void ChangeVibrationRectHeight()
    {
        float height = _vibrationImage.rectTransform.rect.height;
        float width = isVibrationActive ? _vibrationOnIcoHeight : _vibrationOffIcoHeight;

        _vibrationImage.rectTransform.sizeDelta = new Vector2(width, height);
    }


    public void ClickVote()
    {

    }

}
