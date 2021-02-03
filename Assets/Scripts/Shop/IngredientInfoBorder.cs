using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientInfoBorder : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _kcal;
    [SerializeField] private Text _height;
    [SerializeField] private Text _mass;


    public void ShowIngredientInfo(IngredientSO ingredientInfo)
    {
        if (ingredientInfo.isLock)
        {
            _name.text = "???";
            _kcal.text = "";
            _height.text = "";
            _mass.text = "";
        }
        else
        {
            _name.text = ingredientInfo.name;
            _kcal.text = ingredientInfo.kcal + " kcal";
            _height.text = (ingredientInfo.height * 10) + " cm.";
            _mass.text = ingredientInfo.mass + " g.";
        }
    }
}
