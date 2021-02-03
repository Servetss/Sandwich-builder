using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    private AudioSource _audioSource;
    private ParticleSystem _particleSystem;

    private const int _maxPlayCount = 3;
    private int _playCountNow;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponent<ParticleSystem>();
        _playCountNow = 0;
    }

    public void StartFX()
    {
        Invoke("Play", _particleSystem.startDelay);
    }


    private void Play()
    {
        _particleSystem.Play();


        if (_playCountNow < _maxPlayCount)
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }  


        _playCountNow++;

        Invoke("StartFX", _particleSystem.duration);
    }


    private void OnDisable()
    {
        EnableFX();
    }


    private void EnableFX()
    {
        print("Firefarm DISABLE");
        CancelInvoke("StartFX");
        _playCountNow = 0;
    }

}
