using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private ThrowDirection _throwDirectionComp;

    private void Awake()
    {
        _throwDirectionComp = GetComponent<ThrowDirection>();
    }



    public void ChangeSpeed()
    {
        if(Burger.instance.GetIngredientsLength() % GameManager.instance.GetHitsForNextLevel() == 0)
            _throwDirectionComp.ChangeSpeed(GameManager.instance.GetSpeedChangePerLevel());
    }

    public void ChangeDegreeArrwoThrow()
    {
        if (Burger.instance.GetIngredientsLength() % GameManager.instance.GetHitsForNextLevel() == 0)
            _throwDirectionComp.ChangeThrowDiapasone(GameManager.instance.GetArrowDegreeChangePerLevel());
    }

}
