using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBonusBehaviour : MonoBehaviour
{
    [SerializeField] private int _staggeringStopCost;
    [SerializeField] private int _addHealtCost;
    [SerializeField] private int _slowDownArrowCost;


    public void StopStagger()
    {
        if (GameManager.instance.GetScore() >= _staggeringStopCost)
        {
            GameManager.instance.ChangeScore(-_slowDownArrowCost);
            Bonuses.StopStagger();
        }
    }


    public void AddHealtPoint()
    {
        if (GameManager.instance.GetScore() >= _addHealtCost)
        {
            GameManager.instance.ChangeScore(-_slowDownArrowCost);
            Bonuses.AddHealtPoint(1);
        }
    }


    public void SlowDownArrowThrowSpeed()
    {
        if (GameManager.instance.GetScore() >= _slowDownArrowCost)
        {
            GameManager.instance.ChangeScore(-_slowDownArrowCost);
            Bonuses.SlowDownArrowThrowSpeed();
        }
    }
}
