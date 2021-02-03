using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{

    [SerializeField] private Text _money;


    private void Awake()
    {
        Wallet.Start();

        MoneyRenderer();
        Wallet.SubscripeForMoneyChanged(MoneyRenderer);
    }

    public void MoneyRenderer()
    {
        _money.text = Wallet.passiveMoney.ToString();
    }
}
