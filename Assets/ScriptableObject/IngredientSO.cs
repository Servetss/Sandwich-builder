using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class IngredientSO : ScriptableObject
{
    private const string _isLockSave = "IsLock";
    private const string _isSelectedSave = "IsSelected";

    public string modelsName;
    public string name;
    public Sprite sprite;

    [Space]
    public bool isLock;
    public bool isSelected;
    public bool isAlwaysActive;

    [Space]
    public float mass;
    public float height;
    public float kcal;


    public void LoadData()
    {
        isLock = PlayerPrefs.GetInt(_isLockSave + modelsName) == 1;
        isSelected = PlayerPrefs.GetInt(_isSelectedSave + modelsName) == 1;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(_isLockSave + modelsName, isLock ? 1 : 0);
        PlayerPrefs.SetInt(_isSelectedSave + modelsName, isSelected ? 1 : 0);
    }


    public void DefaultSettings()
    {
        if (modelsName == "Bread_Tost" || modelsName == "Cheese" || modelsName == "Patty")
        {
            isLock = false;
            isSelected = true;
        }
        else
        {
            isLock = true;
            isSelected = false;
        }


        SaveData();
    }
}