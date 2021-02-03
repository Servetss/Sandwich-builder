using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtPoint : MonoBehaviour
{
    [SerializeField] private int _healtOnTheStart;
    [SerializeField] private int _healt;

    [SerializeField] private GameObject[] _healthObjects;
    
    
    private void Start()
    {
        SetStartHealth();
    }


    public void AddHealth(int pointAdd)
    {
        _healt += pointAdd;

        if(_healt < _healthObjects.Length)
        _healthObjects[_healt].SetActive(true);
    }

    public void DepriveHealth()
    {
        _healt--;
        _healthObjects[_healt].SetActive(false);

        if (_healt == 0)
        {
            GameOver.instance.GameOverFunk();
            print("Health 0");
        }
    }

    public void SetStartHealth()
    {
        _healt = _healtOnTheStart;

        for (int i = 0; i < _healthObjects.Length; i++)
        {
            _healthObjects[i].SetActive(true);
        }
    }

}
