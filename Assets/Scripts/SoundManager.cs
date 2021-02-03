using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    [Header("Audio source")]
    [SerializeField] private AudioSource _burgerAudioSource;
    [SerializeField] private AudioSource _buttonsAudioSource;


    [Space]
    [Header("Burger interaction")]
    [SerializeField] private AudioClip _greatHit;
    [SerializeField] private AudioClip _ingredientHit;
    [SerializeField] private AudioClip _ingredientFall;

    [Header("Perfect types")]
    [SerializeField] private AudioClip[] _perfect;

    [Space]
    [Header("Main menu")]
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _back;

    [Header("Gift")]
    [SerializeField] private AudioClip _giftFall;
    [SerializeField] private AudioClip _giftHit;
    [SerializeField] private AudioClip _giftOpen;


 

    [Header("Firewark")]
    [SerializeField] private Transform _firewarkTransform;
    private AudioSource[] _firewarksSound;


    private int _perfectHitInRowCount;

    private void Awake()
    {
        instance = this;

        _firewarksSound = new AudioSource[_firewarkTransform.childCount];
        for (int i = 0; i < _firewarkTransform.childCount; i++)
        {
            _firewarksSound[i] = _firewarkTransform.GetChild(i).GetComponent<AudioSource>();
        }

        _perfectHitInRowCount = 0;
    }


    #region Settings sounds
    public void SetAudioSorceMute(bool soundActive)
    {
        _burgerAudioSource.mute = !soundActive;
        _buttonsAudioSource.mute = !soundActive;


        for (int i = 0; i < _firewarksSound.Length; i++)
        {
            _firewarksSound[i].mute = !soundActive;
        }
    }


    



    public void Vibrate()
    {
        if (Settings.instance.isVibrationActive)
        {
            Handheld.Vibrate();
        }

    }
    #endregion


    public void GiftHit()
    {
        _burgerAudioSource.PlayOneShot(_giftHit);
    }

    public void GiftFall()
    {
        _burgerAudioSource.PlayOneShot(_giftFall);
    }

    public void GiftOpen()
    {
        _burgerAudioSource.PlayOneShot(_giftOpen);
    }


    #region burger interaction

    public void FirstHit()
    {
        //_burgerAudioSource.clip = _ingredientHit;
        //_burgerAudioSource.Play();
    }


    public void PerfectHitTypesSound()
    {
        _burgerAudioSource.PlayOneShot(_perfect[_perfectHitInRowCount]);

        _perfectHitInRowCount++;

        if (_perfect.Length == _perfectHitInRowCount) _perfectHitInRowCount = 0;
    }

    public void PerfectHitSound()
    {
        _burgerAudioSource.PlayOneShot(_greatHit);
        _burgerAudioSource.PlayOneShot(_ingredientHit);
    }


    public void BurgerHitSound()
    {
        _burgerAudioSource.clip = _ingredientHit;
        _burgerAudioSource.Play();

        _perfectHitInRowCount = 0;
    }

    public void BurgerFallSound()
    {
        _burgerAudioSource.clip = _ingredientFall;
        _burgerAudioSource.Play();

        _perfectHitInRowCount = 0;
    }
    
    
    
    


    #endregion


    #region buttons sounds
    public void ClickSelect()
    {
        _buttonsAudioSource.clip = _click;
        _buttonsAudioSource.Play();
    }

    public void ClickBack()
    {
        _buttonsAudioSource.clip = _back;
        _buttonsAudioSource.Play();
    }
    #endregion
}
