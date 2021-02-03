using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Wallet
{
    private const string _moneySave = "Money";

    public static int passiveMoney { get; private set; }
    public static int activeMoney { get; private set; }

    public delegate void MoneyChanged();
    private static MoneyChanged _moneyChanged;


    public static void Start()
    {
        // PlayerPrefs.SetInt(_moneySave,0);
        passiveMoney = PlayerPrefs.GetInt(_moneySave);
        _moneyChanged = SaveMoney;
    }

    public static void AddActiveMoney()
    {
        activeMoney++;
    }

    public static void Buy(int price)
    {
        passiveMoney -= price;
        _moneyChanged();
    }

    public static void AddPassiveMoney(int value)
    {
        passiveMoney += value;
        
        _moneyChanged();
    }


    public static void MoveActiveMoneyToPassive()
    {
        AddPassiveMoney(activeMoney);
        activeMoney = 0;
    }


    public static void SubscripeForMoneyChanged(MoneyChanged function)
    {
        if (_moneyChanged == null)
        {
            _moneyChanged = function;
        }
        else
        {
            _moneyChanged += function;
        }
    }

    public static void UnsubscripeForMoneyChanged(MoneyChanged function)
    {
        _moneyChanged -= function;
    }


    public static void SaveMoney()
    {
        PlayerPrefs.SetInt(_moneySave, passiveMoney);
    }


}
