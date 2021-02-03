using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bonuses
{
    public static void StopStagger()
    {
        Burger.instance.ChangeDegreeOfStagger(Burger.instance.GetStaggerDegree() * -4);

        Debug.Log("STAGGER 0");
    }


    public static void AddHealtPoint(int pointAdd)
    {
        GameManager.instance.AddHealth(pointAdd);

        Debug.Log("Add " + pointAdd + " point");
    }


    public static void SlowDownArrowThrowSpeed()
    {
        ThrowDirection.instance.ChangeSpeed(-(ThrowDirection.instance.GetArrowSpeed() / 2));

        Debug.Log("SLOW DOWN ARROW SPEED");
    }
}
